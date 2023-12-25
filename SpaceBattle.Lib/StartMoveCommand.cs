using Hwdtech;

namespace SpaceBattle.Lib;

public class StartMoveCommand: ICommand
{
    private readonly ICommandStartable _mcs;

    public StartMoveCommand(ICommandStartable mcs)
    {
        _mcs = mcs;
    }

    public void Execute()
    {
        _mcs.Properties.ToList().ForEach(property => IoC.Resolve<object>(
            "Game.IUObject.SetProperty",
            _mcs.uobject,
            property.Key,
            property.Value
        ));

        
        //var cmd = IoC.Resolve<ICommand>()
    }
}
