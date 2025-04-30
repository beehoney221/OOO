using Hwdtech;
using Hwdtech.Ioc;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

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
}
