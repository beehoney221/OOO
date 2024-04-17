using Hwdtech;

namespace SpaceBattle.Lib;

public class RegistInterpretCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Command.Interpretation",
        (object[] args) =>
        {
            var contract = (Contract)args[0];

            var cmdGame = new ActionCommand(() =>
            {
                var obj = IoC.Resolve<object>($"Game.Object.Get", contract.gameItemId);
                var command = IoC.Resolve<ICommand>($"Game.Command.{contract.type}", obj, contract);
                IoC.Resolve<ICommand>("Game.Command.Add", command).Execute();
            });

            return cmdGame;
        }
        ).Execute();
    }
}