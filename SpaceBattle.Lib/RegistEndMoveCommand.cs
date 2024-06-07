using Hwdtech;

namespace SpaceBattle.Lib;

public class RegistEndMoveCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.EndMove",
            (object[] args) =>
            {
                var obj = args[0];
                var parameters = (IDictionary<string, object>)args[1];
                
                var cmd = IoC.Resolve<ICommand>("Command.EndMove", obj, parameters);
                
                return cmd;
            }
        ).Execute();
    }
}