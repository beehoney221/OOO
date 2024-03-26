namespace SpaceBattle.Lib;

public class HardStopCommand : ICommand
{
    private ServerThread _st;
    public HardStopCommand(ServerThread st) {
        _st = st;
    }
    public void Execute()
    {
        _st.Stop();
    }
}