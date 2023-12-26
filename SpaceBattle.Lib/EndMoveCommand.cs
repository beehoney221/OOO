using Hwdtech;

namespace SpaceBattle.Lib;

public class EndMoveCommand : ICommand
{
    private readonly IMoveCommandEndable _end;

    public EndMoveCommand(IMoveCommandEndable end)
    {
        _end = end;
    }

    public void Execute()
    {
        var emptyCmd = IoC.Resolve<ICommand>("Cmd.Empty");
        _end.Property.ToList().ForEach(value => IoC.Resolve<ICommand>("DelProperty", _end.Object, value).Execute());
        var cmd = IoC.Resolve<IBridgeCommand>("GetProperty", _end.Object, _end.Command);
        IoC.Resolve<IBridgeCommand>("Cmd.Inject", cmd, emptyCmd);
    }
}
