using Hwdtech;

namespace SpaceBattle.Lib;
public class CreateTorpedoCommand : ICommand
{
    private readonly Vector _position;
    private readonly Vector _velocity;

    public CreateTorpedoCommand(Vector position, Vector velocity)
    {
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
