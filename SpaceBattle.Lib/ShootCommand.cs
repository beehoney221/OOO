using Hwdtech;
namespace SpaceBattle.Lib;
public class ShootCommand : ICommand
{
    private readonly IShootable _shootable;
    public ShootCommand(IShootable shootable) => _shootable = shootable;
    public void Execute()
    {
        new CreatTorpedo.Create(_shootable)
        var torpedo = IoC.Resolve<IUObject>("Game.Create.Torpedo", _shootable.torpedoPosition, _shootable.torpedoVelocity);
        var cmdStartMove = IoC.Resolve<ICommand>("Game.Command.StartMove", torpedo);
        cmdStartMove.Execute();
    }
}
