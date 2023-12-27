using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Tests;
public class ActionCommand : Lib.ICommand
{
    private readonly Action _action;
    public ActionCommand(Action action)
    {
        _action = action;
    }

    public void Execute()
    {
        _action();
    }
}

public class StarMoveCommandTests
{
    public StarMoveCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set", 
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Gsme.IUObject.SetProperty",
            (object[] args) =>
            {
                var target = (IUObject)args[0];
                var key = (string)args[1];
                var value = args[2];

                target.SetProperty(key, value);
                return new object();
            }
        ).Execute();

        var startmc = new Mock<ICommand>();

        IoC.Resolve<ICommand>(
            "IoC.Register",
            "Game.Commands.StartMove",
             (object[] args) =>
            {
                return startmc;
            }
        ).Execute();

        IoC.Resolve<ICommand>(
            "IoC.Register",
            "Game.Commands.Inject.StartMove",
            (object[] args) =>
            {
                return args[0];
            }
        ).Execute();

    }
}