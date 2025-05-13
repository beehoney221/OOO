using Hwdtech;

namespace SpaceBattle.Lib;
public class CreateTorpedoCommand : ICommand
{
    private readonly IMovable _ship;
    private readonly Vector _position;
    private readonly Vector _velocity;

    public CreateTorpedoCommand(IMovable ship, Vector position, Vector velocity)
    {
        _ship = ship;
        _position = position;
        _velocity = velocity;
    }

    public void Execute()
    {
        var torpedo = new Torpedo(_position, _velocity);

        var cmdStartMove =  IoC.Resolve<SpaceBattle.Lib.ICommand>("Game.StartMoveCommand", torpedo);
        
        cmdStartMove.Execute();
    }
}
