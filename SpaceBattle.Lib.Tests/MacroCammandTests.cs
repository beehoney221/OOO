using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Tests;

public class MacroCommandTests
{
    public MacroCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();

        new MacroCommandBuild().Execute();

        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.MacroCommands.MoveWithShoot", (object[] args) =>
        {
            return new string[] { "Game.Commands.Move", "Game.Commands.Shoot" };
        }).Execute();
    }

    [Fact]
    public void MacroCommand_Positive()
    {
        var moveCommand = new Mock<ICommand>();
        moveCommand.Setup(mc => mc.Execute()).Callback(() => { }).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Move", (object[] args) =>
        {
            return moveCommand.Object;
        }).Execute();

        var shootCommand = new Mock<ICommand>();
        shootCommand.Setup(cfc => cfc.Execute()).Callback(() => { }).Verifiable();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.Commands.Shoot", (object[] args) =>
        {
            return shootCommand.Object;
        }).Execute();

        var macroCommand = (MacroCommand)IoC.Resolve<Lib.ICommand>("Game.MacroCommand.Build", "Game.MacroCommands.MoveWithShoot");
        macroCommand.Execute();

        moveCommand.Verify(mc => mc.Execute(), Times.Once());
        shootCommand.Verify(sc => sc.Execute(), Times.Once());
    }
}
