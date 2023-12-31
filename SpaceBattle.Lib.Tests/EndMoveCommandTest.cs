using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Tests;
public class EndMoveCommandTest
{
    public EndMoveCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var moqCmd = new Mock<Lib.ICommand>();
        moqCmd.Setup(mc => mc.Execute());

        var moqInject = new Mock<IBridgeCommand>();
        moqInject.Setup(mc => mc.Inject(It.IsAny<Lib.ICommand>()));

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Cmd.Empty",
            (object[] args) =>
            {
                return new EmptyCommand();
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Cmd.Inject",
            (object[] args) =>
            {
                var locInject = (IBridgeCommand)args[0];
                var cmdInject = (Lib.ICommand)args[1];
                locInject.Inject(cmdInject);

                return locInject;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Cmd.End",
            (object[] args) =>
            {
                var mCmd = (IMoveCommandEndable)args[0];

                return new EndMoveCommand(mCmd);
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "GetProperty",
            (object[] args) =>
            {
                return moqInject.Object;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "DelProperty",
            (object[] args) =>
            {
                return moqCmd.Object;
            }
        ).Execute();
    }

    [Fact]
    public void EndCommandTest()
    {
        var moqEnd = new Mock<IMoveCommandEndable>();
        var moqObject = new Mock<IUObject>();

        moqEnd.Setup(i => i.Object).Returns(moqObject.Object).Verifiable();
        moqEnd.Setup(i => i.Command).Returns("Rotate").Verifiable();
        moqEnd.Setup(i => i.Property).Returns(new List<string> { "Angle" }).Verifiable();

        IoC.Resolve<ICommand>("Cmd.End", moqEnd.Object).Execute();
        moqEnd.VerifyAll();
    }

    [Fact]
    public void BridgeCommandTest()
    {
        var moqCmd_1 = new Mock<ICommand>();
        moqCmd_1.Setup(x => x.Execute()).Verifiable();

        var moqCmd_2 = new Mock<ICommand>();
        moqCmd_2.Setup(x => x.Execute()).Verifiable();

        var bridgeCmd = new BridgeCommand(moqCmd_1.Object);
        bridgeCmd.Execute();

        moqCmd_2.Verify(m => m.Execute(), Times.Never());
        bridgeCmd.Inject(moqCmd_2.Object);
        bridgeCmd.Execute();

        moqCmd_2.Verify(m => m.Execute(), Times.Once());
    }

    [Fact]
    public void EmptyCommandTest()
    {
        var empCmd = IoC.Resolve<ICommand>("Cmd.Empty");

        empCmd.Execute();
        Assert.NotNull(empCmd);
    }
}
