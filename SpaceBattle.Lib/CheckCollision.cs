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
        var positionFirst = IoC.Resolve<List<int>>("Game.IUObject.GetProperty", _objectFirts, "Position");
        var velocityFirst = IoC.Resolve<List<int>>("Game.IUObject.GetProperty", _objectFirts, "Velocity");
        var positionSecond = IoC.Resolve<List<int>>("Game.IUObject.GetProperty", _objectSecond, "Position");
        var velocitySecond = IoC.Resolve<List<int>>("Game.IUObject.GetProperty", _objectSecond, "Velocity");

        var newcoord = positionFirst.Select((value, index) => value - positionSecond[index]).Concat(
            velocityFirst.Select((value, index) => value - velocitySecond[index])
        ).ToArray();
        var tree =IoC.Resolve<IDictionary<int, object>>("Game.BuildTree");

        newcoord.ToList().ForEach(newcoord => tree = (IDictionary<int, object>)tree[newcoord]);

        IoC.Resolve<ICommand>("Game.Collision", _objectFirts, _objectSecond).Execute();

    }
}