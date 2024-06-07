using Hwdtech;

namespace SpaceBattle.Lib;

public class RegistFireCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.Fire",
            (object[] args) =>
            {
                var obj = args[0];
                var parameters = (IDictionary<string, object>)args[1];
                
                var cmd = IoC.Resolve<ICommand>("Command.Fire", obj, parameters);
                
                return cmd;
            }
        ).Execute();
    }
}