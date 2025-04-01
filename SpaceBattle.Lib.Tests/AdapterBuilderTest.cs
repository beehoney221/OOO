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
        var expected =
@"public class IMovableAdapter : IMovable
{
    readonly private IUObject _obj;
    public IMovableAdapter(IUObject obj) => _obj = obj;

    public Vector Position
    {

        get => IoC.Resolve<Vector>(""Game.Get.Property"", ""Position"", _obj);


        set => IoC.Resolve<ICommand>(""Game.Set.Property"", ""Position"", _obj, value).Execute();

    }

    public Vector Velocity
    {

        get => IoC.Resolve<Vector>(""Game.Get.Property"", ""Velocity"", _obj);


    }

}";
        new AdapterBuilder().Builder();
        var targetType = typeof(IUObject);
        var newTargetType = typeof(IMovable);

        var result = IoC.Resolve<string>("Game.Adapter.Build", newTargetType, targetType);

        Assert.Equal(expected, result);
    }
}
