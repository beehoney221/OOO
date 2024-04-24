using Hwdtech;

namespace SpaceBattle.Lib;

public class MessageHandler : ICommand
{
    private readonly IContract _contract;
    public MessageHandler(IContract contract)
    {
        _contract = contract;
    }
    public void Execute()
    {
        var cmdGame = new ActionCommand(() =>
        {
            var obj = IoC.Resolve<object>("Game.Object.Get", _contract.gameItemId);
            try
            {
                var command = IoC.Resolve<ICommand>($"Game.Command.{_contract.type}", obj, _contract.parameters);
                IoC.Resolve<ICommand>("Game.Queue.Add", _contract.gameId, command).Execute();
            }
            catch (ArgumentException)
            {
                throw new Exception($"IoC dependency with key \"Game.Command.{_contract.type}\" is not found.");
            }
            catch
            {
                throw new Exception($"Game witn ID '{_contract.gameId}' is not found.");
            }
        });

        cmdGame.Execute();
    }
}
