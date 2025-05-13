using Hwdtech;
using SpaceBattle.Lib;

namespace SpaceBattle;
public class CreateTorpedoCommand : ICommand
{
    private readonly IMovable _ship;
    private readonly IVector _position;
    private readonly IVector _velocity;

    public CreateTorpedoCommand(IMovable ship, IVector position, IVector velocity)
    {
        _ship = ship;
        _position = position;
        _velocity = velocity;
    }

    public void Execute()
    {
        var torpedo = new Torpedo(_position, _velocity);

        IoC.Resolve<ICommand>("Game.RegisterObject", torpedo).Execute();

        var moveCommand = new MoveCommand(torpedo);
        IoC.Resolve<ICommand>("Game.AddCommand", moveCommand).Execute();
    }
}
