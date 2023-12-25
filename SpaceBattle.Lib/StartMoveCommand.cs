namespace SpaceBattle.Lib;

public class StartMoveCommand: ICommand
{
    private readonly IMoveCommandStartable _mcs;

    public StartMoveCommand(IMoveCommandStartable mcs)
    {
        _mcs = mcs;
    }

    public void Execute()
    {
        
    }
}
