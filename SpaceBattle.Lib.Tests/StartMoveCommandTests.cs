using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Tests;

public class StartMoveCommandTests
{
    public StartMoveCommandTests()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>(
            "Scopes.Current.Set",
            IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.IUObject.SetProperty",
            (object[] args) =>
            {
                var target = (IUObject)args[0];
                var key = (string)args[1];
                var value = args[2];

                target.SetProperty(key, value);
                return new object();
            }
        ).Execute();

        var startmc = new Mock<ICommand>().Object;

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Commands.StartMove",
             (object[] args) =>
            {
                return startmc;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Commands.Bridge",
            (object[] args) =>
            {
                return new BridgeCommand((ICommand)args[0]);
            }
        ).Execute();

    }

    [Fact]

    public void StartMoveCommand_Succefully()
    {
        var qMock = new Mock<IQueue>();
        var qReal = new Queue<ICommand>();

        qMock.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(qReal.Enqueue);

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Queue",
            (object[] args) =>
            {
                return qMock.Object;
            }
        ).Execute();

        var moveStartable = new Mock<ICommandStartable>();
        var target = new Mock<IUObject>();
        var targets = new Dictionary<string, object>();
        var properties = new Dictionary<string, object> {
            {"Velocity", new Vector(new int[] { 12, 5 })},
        };

        moveStartable.SetupGet(s => s.Properties).Returns(properties);
        moveStartable.SetupGet(s => s.Target).Returns(target.Object);
        target.Setup(o => o.SetProperty(It.IsAny<string>(), It.IsAny<object>())).Callback<string, object>(targets.Add);
        qMock.Setup(q => q.Add(It.IsAny<ICommand>())).Callback(qReal.Enqueue);

        var startMoveCommand = new StartMoveCommand(moveStartable.Object);
        startMoveCommand.Execute();

        Assert.Contains("Velocity", targets.Keys);
        Assert.Contains("Game.Commands.Bridge.StartMove", targets.Keys);
        Assert.NotEmpty(qReal);
    }

    [Fact]
    public void BridgeTest()
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
}
