using Hwdtech;
using Hwdtech.Ioc;

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
        new AdapterBuilder().Builder();
        var targetType = typeof(IUObject);
        var newTargetType = typeof(IMovable);

        var result = IoC.Resolve<string>("Game.Adapter.Build", newTargetType, targetType);

        Assert.Contains("public class IMovableAdapter : IMovable", result);
        Assert.Contains("readonly private IUObject _obj;", result);
        Assert.Contains("public IMovableAdapter(IUObject obj) => _obj = obj;", result);

        Assert.Contains("public Vector Position", result);
        Assert.Contains("get => IoC.Resolve<Vector>(\"Game.Get.Property\", \"Position\", _obj);", result);
        Assert.Contains("set => IoC.Resolve<ICommand>(\"Game.Set.Property\", \"Position\", _obj, value).Execute();", result);

        Assert.Contains("public Vector Velocity", result);
        Assert.Contains("get => IoC.Resolve<Vector>(\"Game.Get.Property\", \"Velocity\", _obj);", result);
    }
}
