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
        var obj = IoC.Resolve<object>("Game.Object.Get", _contract.gameItemId);

        var command = IoC.Resolve<ICommand>($"Game.Command.{_contract.type}", obj, _contract.parameters);

        IoC.Resolve<ICommand>("Game.Queue.Add", _contract.gameId, command).Execute();
    }
}
