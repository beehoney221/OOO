using Hwdtech;

namespace SpaceBattle.Lib;

public class RegistInterpretCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "InterpretCommand",
        (object[] args) =>
        {
            var contract = (Contract)args[0];

            var cmd = new ActionCommand(() =>
            {
                IoC.Resolve<ICommand>("CreateCommand", contract);
                IoC.Resolve<ICommand>($"Commands.{contract.cmdType}", contract); /// зачем это надо
            });

            /// поместить в очередь соответствующей игры

            return cmd;
        }
        ).Execute();
    }
}