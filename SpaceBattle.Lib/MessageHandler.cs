using Hwdtech;

namespace SpaceBattle.Lib;

public class MessageHandler : ICommand
{
    private readonly Contract _contract;
    public MessageHandler(Contract contract)
    {
        _contract = contract;
    }
    public void Execute()
    {
        new RegistInterpretCommand().Execute();

        var commandInterpretation = IoC.Resolve<ICommand>("Command.Interpretation", _contract);

        commandInterpretation.Execute();
    }
}
