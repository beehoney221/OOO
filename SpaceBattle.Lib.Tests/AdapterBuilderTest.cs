using System.Reflection;
using Hwdtech;
using Hwdtech.Ioc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Moq;

namespace SpaceBattle.Lib.Tests;

public class AdapterBuilderTest
{
    public AdapterBuilderTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
    }

    [Fact]
    public void PositiveBuildingAdapter()
    {
        var builder = new AdapterBuilder();
        builder.Builder();

        var targetType = typeof(IUObject);
        var newTargetType = typeof(IMovable);

        var generatedCode = IoC.Resolve<string>("Game.Adapter.Build", newTargetType, targetType);
        var syntaxTree = CSharpSyntaxTree.ParseText(generatedCode);

        var diagnostics = syntaxTree.GetDiagnostics();
        Assert.DoesNotContain(diagnostics, d => d.Severity == DiagnosticSeverity.Error);

        var root = syntaxTree.GetRoot();

        var classDeclarations = root.DescendantNodes().OfType<ClassDeclarationSyntax>();
        Assert.Contains(classDeclarations, cd => cd.Identifier.Text == "IMovableAdapter");

        var constructors = root.DescendantNodes().OfType<ConstructorDeclarationSyntax>();
        Assert.Contains(constructors, c => c.ParameterList.Parameters.Count == 1);

        var properties = root.DescendantNodes().OfType<PropertyDeclarationSyntax>();
        Assert.Contains(properties, p => p.Identifier.Text == "Position");
        Assert.Contains(properties, p => p.Identifier.Text == "Velocity");
    }

    [Fact]
    public void Adapter_ShouldCorrectlyWork()
    {
        new AdapterBuilder().Builder();

        var initialPosition = new Vector(new int[] { 12, 5 });
        var velocity = new Vector(new int[] { -7, 3 });
        var expectedNewPosition = initialPosition + velocity;

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Get.Property", (object[] args) =>
        {
            var propertyName = args[0];
            var obj = args[1];

            return propertyName switch
            {
                "Position" => initialPosition,
                "Velocity" => velocity,
                _ => throw new Exception($"Unknown property: {propertyName}")
            };
        }).Execute();

        var setPropertyCalled = false;
        var actualNewPosition = new Vector(new int[] { 0, 0 });

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Set.Property", (object[] args) =>
        {
            var propertyName = args[0];
            var obj = args[1];
            var value = (Vector)args[2];

            return new ActionCommand(() =>
            {
                setPropertyCalled = true;
                actualNewPosition = value;
            });
        }).Execute();

        var adapterCode = IoC.Resolve<string>("Game.Adapter.Build", typeof(IMovable), typeof(IUObject));
        var assembly = CompileAdapter(adapterCode);
        var adapterType = assembly.GetTypes().First(t => t.Name == "IMovableAdapter");
        var adapter = Activator.CreateInstance(adapterType, new Mock<IUObject>().Object) as IMovable 
            ?? throw new InvalidOperationException("Failed to create adapter");

        var moveCommand = new MoveCommand(adapter);
        moveCommand.Execute();

        Assert.True(setPropertyCalled, "Команда MoveCommand не выполнила SetProperty");
        Assert.Equal(expectedNewPosition, actualNewPosition);
    }


    private static Assembly CompileAdapter(string code)
    {
        var systemRuntime = typeof(object).Assembly.Location;
        var iobjectAssembly = typeof(IUObject).Assembly.Location;
        var commandAssembly = typeof(ICommand).Assembly.Location;
        var vectorAssembly = typeof(Vector).Assembly.Location;
        var hwdtechAssembly = typeof(Hwdtech.IoC).Assembly.Location;
        var movableAssembly = typeof(IMovable).Assembly.Location;
        
        var references = new[]
        {
        MetadataReference.CreateFromFile(systemRuntime),
        MetadataReference.CreateFromFile(iobjectAssembly),
        MetadataReference.CreateFromFile(commandAssembly),
        MetadataReference.CreateFromFile(vectorAssembly),
        MetadataReference.CreateFromFile(hwdtechAssembly),
        MetadataReference.CreateFromFile(movableAssembly),
        MetadataReference.CreateFromFile(Assembly.Load("System.Runtime").Location),
        MetadataReference.CreateFromFile(Assembly.Load("netstandard").Location),
        MetadataReference.CreateFromFile(Assembly.LoadFrom("SpaceBattle.Lib.Tests.dll").Location)
    };

        var fullCode = @"
using System;
using Hwdtech;
using static Hwdtech.IoC;
namespace SpaceBattle.Lib.Test;
" + code;

        var compilation = CSharpCompilation.Create(
            "TempAdapterAssembly",
            new[] { CSharpSyntaxTree.ParseText(fullCode) },
            references,
            new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary));

        using var ms = new MemoryStream();
        var result = compilation.Emit(ms);
        ms.Seek(0, SeekOrigin.Begin);
        return Assembly.Load(ms.ToArray());
    }
}
