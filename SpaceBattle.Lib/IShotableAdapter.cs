using Hwdtech;

namespace SpaceBattle.Lib;

public class IShootableAdapter : IShootable
{
    private readonly IUObject _obj;

    public IShootableAdapter(IUObject obj) => _obj = obj;

    public Vector torpedoPosition
    {
        get => IoC.Resolve<Vector>("Game.Get.Property", _obj, "Position");
        set => IoC.Resolve<ICommand>("Game.Set.Property", _obj, "Position", value).Execute();
    }
    public Vector torpedoVelocity => IoC.Resolve<Vector>("Game.Get.Property", _obj, "Velocity");
}
