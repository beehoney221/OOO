using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Tests;
public class MessageHandlerTest
{
    public Dictionary<int, IUObject> _idObj = new Dictionary<int, IUObject>();
    public Dictionary<string, Queue<ICommand>> _gameQueue = new Dictionary<string, Queue<ICommand>>();
    public Mock<ICommand> _cmdAdd = new();
    public MessageHandlerTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Get.IdObject",
        (object[] args) =>
        {
            return _idObj;
        }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Get.GameQueue",
        (object[] args) =>
        {
            return _gameQueue;
        }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Queue.Add",
        (object[] args) =>
        {
            var gameId = (string)args[0];
            var commandForQ = (ICommand)args[1];
            var queue = IoC.Resolve<Queue<ICommand>>("Game.Queue.Get", gameId);

            _cmdAdd.Setup(c => c.Execute()).Callback(() => commandForQ.Execute()).Verifiable();

            return _cmdAdd.Object;
        }
        ).Execute();

        var rotatable = new Mock<IRotatable>();
        rotatable.SetupGet(x => x.Angle).Returns(new Angle(90));

        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Command.Rotate",
        (object[] args) =>
        {
            var obj = (IUObject)args[0];
            var parameters = (IDictionary<string, object>)args[1];

            rotatable.SetupGet(x => x.AngleVelocity).Returns(new Angle((int)parameters["angle velocity"]));

            var cmdRotate = new RotateCommand(rotatable.Object);

            return cmdRotate;
        }
        ).Execute();

        new RegistObjectGet().Execute();
        new RegistQueueGet().Execute();
    }

    [Fact]
    public void HandlerSuccesful()
    {
        var type = "Rotate";
        var gameId = "asdfg";
        var gameItemId = 548;
        var parameters = new Dictionary<string, object>() { { "angle velocity", 50 } };

        var contract = new Mock<IContract>();
        contract.SetupGet(x => x.type).Returns(type);
        contract.SetupGet(x => x.gameId).Returns(gameId);
        contract.SetupGet(x => x.gameItemId).Returns(gameItemId);
        contract.SetupGet(x => x.parameters).Returns(parameters);

        var moqObj = new Mock<IUObject>();
        _idObj.Add(gameItemId, moqObj.Object);

        _gameQueue.Add(gameId, new Queue<ICommand>());

        var mh = new MessageHandler(contract.Object);
        mh.Execute();

        _cmdAdd.Verify(c => c.Execute(), Times.Once());
    }

    [Fact]
    public void ObjectNotFound()
    {
        var gameItemId = 548;
        var contract = new Mock<IContract>();
        contract.SetupGet(x => x.gameItemId).Returns(gameItemId);

        var mh = new MessageHandler(contract.Object);
        var exc = Assert.Throws<Exception>(mh.Execute);

        Assert.Equal($"Game object with ID '{gameItemId}' is not found.", exc.Message);
    }

    [Fact]
    public void DependencyNotFound()
    {
        var type = "StartMovement";
        var gameItemId = 548;
        var parameters = new Dictionary<string, object>() { { "initial velocity", 2 } };

        var contract = new Mock<IContract>();
        contract.SetupGet(x => x.type).Returns(type);
        contract.SetupGet(x => x.gameItemId).Returns(548);
        contract.SetupGet(x => x.parameters).Returns(parameters);

        var moqObj = new Mock<IUObject>();
        _idObj.Add(gameItemId, moqObj.Object);

        var mh = new MessageHandler(contract.Object);
        var exc = Assert.Throws<Exception>(mh.Execute);

        Assert.Equal($"IoC dependency with key \"Game.Command.{type}\" is not found.", exc.Message);
    }

    [Fact]
    public void GameQueueNotFound()
    {
        var type = "Rotate";
        var gameId = "qwert";
        var gameItemId = 548;
        var parameters = new Dictionary<string, object>() { { "angle velocity", 50 } };

        var contract = new Mock<IContract>();
        contract.SetupGet(x => x.type).Returns(type);
        contract.SetupGet(x => x.gameId).Returns(gameId);
        contract.SetupGet(x => x.gameItemId).Returns(gameItemId);
        contract.SetupGet(x => x.parameters).Returns(parameters);

        var moqObj = new Mock<IUObject>();
        _idObj.Add(gameItemId, moqObj.Object);

        var mh = new MessageHandler(contract.Object);
        var exc = Assert.Throws<Exception>(mh.Execute);

        Assert.Equal($"Game witn ID '{gameId}' is not found.", exc.Message);
    }
}
