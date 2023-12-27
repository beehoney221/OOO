using Hwdtech;

namespace SpaceBattle.Lib;

public class StartMoveCommand : ICommand
{
    private readonly ICommandStartable _smc;

    public StartMoveCommand(ICommandStartable smc)
    {
        _smc = smc;
    }

    public void Execute()
    {
        _smc.Properties.ToList().ForEach(property => IoC.Resolve<object>(
            "Game.IUObject.SetProperty",
            _smc.Target,
            property.Key,
            property.Value
        ));

        var startCmd = IoC.Resolve<ICommand>("Game.Commands.StartMove", _smc.Target);

        var injectCmd = IoC.Resolve<IBridgeCommand>("Game.Commands.Bridge", startCmd);

        IoC.Resolve<object>("Game.IUObject.SetProperty", _smc.Target, "Game.Commands.Bridge.StartMove", injectCmd);

        IoC.Resolve<IQueue>("Game.Queue").Add((ICommand)startCmd);
    }
}
