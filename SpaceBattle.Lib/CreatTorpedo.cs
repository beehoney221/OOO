using Hwdtech;
namespace SpaceBattle.Lib;

public class CreatTorpedo
{
    public void Create(IShootable shoot)
    {
        var torpedoPosition = shoot.torpedoPosition;
        var torpedoVelocity = shoot.torpedoVelocity;

        IoC.Resolve<ICommand>(
            "IoC.Register",
            "Game.Create.Torpedo",
            (object[] args) =>
            {
                var position = (Vector)args[0];
                var velocity = (Vector)args[1];

                var torpedo = IoC.Resolve<IUObject>("Game.New.UObject");
                torpedo.SetProperty("Position", position);
                torpedo.SetProperty("Velocity", velocity);

                return torpedo;
            }
        ).Execute();
    }
}