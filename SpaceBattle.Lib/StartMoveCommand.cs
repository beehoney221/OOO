using Hwdtech;

namespace SpaceBattle.Lib;

public class StarMovetCommand: ICommand
{
    private readonly ICommandStartable _smc;

    public StarMovetCommand(ICommandStartable smc)
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

        var startCmd  = IoC.Resolve<ICommand>("Game.Commands.LongMove", _smc.Target);

        var injectCmd = IoC.Resolve<ICommand>("Game.Commands.Inject", startCmd);

        IoC.Resolve<ICommand>("Game.IUObject.SetProperty", _smc.Target, "Game.Commands.Inject.LongMove", injectCmd);

        IoC.Resolve<IQueue>("Game.Queue").Add((ICommand)injectCmd);
    }
}
