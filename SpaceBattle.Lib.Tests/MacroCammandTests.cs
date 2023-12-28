using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class MacroCommandTests
{
    public MacroCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.MacroCommands.MoveWithShoot", (object[] args) =>
        {
            return new string[] { "Game.Commands.Move", "Game.Commands.Shoot" };
        }).Execute();
    }
}