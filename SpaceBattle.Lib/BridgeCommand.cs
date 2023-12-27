namespace SpaceBattle.Lib;
public class BridgeCommand : ICommand, IBridgeCommand
{
    private ICommand _cmd;
    public BridgeCommand(ICommand cmd)
    {
        _cmd = cmd;
    }

    public void Inject(ICommand command)
    {
        _cmd = command;
    }

    public void Execute()
    {
        _cmd.Execute();
    }
}