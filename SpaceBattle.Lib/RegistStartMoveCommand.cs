using Hwdtech;

namespace SpaceBattle.Lib;

public class RegistStartMoveCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.StartMove",
            (object[] args) =>
            {
                var obj = args[0];
                var parameters = (IDictionary<string, object>)args[1];
                
                var cmd = IoC.Resolve<ICommand>("Command.StartMove", obj, parameters);
                
                return cmd;
            }
        ).Execute();
    }
}
