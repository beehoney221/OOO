using Hwdtech;

namespace SpaceBattle.Lib;

public class CheckCollisionCommand: ICommand
{
    private readonly IUObject _objectFirts, _objectSecond;
    public CheckCollisionCommand(IUObject object1, IUObject object2)
    {
        _objectFirts = object1;
        _objectSecond= object2;
    }
    public void Execute()
    {
        var positionFirst = IoC.Resolve<Vector>("GameIUObject.GetProperty", _objectFirts, "Position");
        var velocityFirst = IoC.Resolve<Vector>("GameIUObject.GetProperty", _objectFirts, "Velocity");
        var positionSecond = IoC.Resolve<Vector>("GameIUObject.GetProperty", _objectSecond, "Position");
        var velocitySecond = IoC.Resolve<Vector>("GameIUObject.GetProperty", _objectSecond, "Velocity");

        

        
    }
}