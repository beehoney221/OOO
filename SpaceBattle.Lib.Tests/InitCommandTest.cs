using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Test;

public class InitCommandTest
{
    public Dictionary<int, IUObject> _idObj = new Dictionary<int, IUObject>();
    public Dictionary<string, Queue<ICommand>> _gameQueue = new Dictionary<string, Queue<ICommand>>();
    public InitCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        new RegistObjectGet().Execute();
        new RegistQueueGet().Execute();

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

            new RegistQueuePut(gameId).Execute();

            var cmdAdd = IoC.Resolve<ICommand>("Game.Queue.Put", commandForQ);

            return cmdAdd;
        }
        ).Execute();

    }

    [Fact]
    public void CreateRotateCommandSuccesful()
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

        new RegistRotateCommand().Execute();

        var rotatable = new Mock<IRotatable>();
        rotatable.SetupGet(x => x.Angle).Returns(new Angle(90));

        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Command.Rotate",
        (object[] args) =>
        {
            var obj = (IUObject)args[0];
            var param = (IDictionary<string, object>)args[1];

            rotatable.SetupGet(x => x.AngleVelocity).Returns(new Angle((int)param["angle velocity"]));

            var cmdRotate = new RotateCommand(rotatable.Object);

            return cmdRotate;
        }
        ).Execute();

        var mh = new MessageHandler(contract.Object);
        mh.Execute();

        var cmd = _gameQueue[gameId].Dequeue();

        Assert.Equal(typeof(RotateCommand), cmd.GetType());
    }

    [Fact]
    public void CreateFireCommandSuccesful()
    {
        var type = "Fire";
        var gameId = "asdfg";
        var gameItemId = 548;
        var parameters = new Dictionary<string, object>();

        var contract = new Mock<IContract>();
        contract.SetupGet(x => x.type).Returns(type);
        contract.SetupGet(x => x.gameId).Returns(gameId);
        contract.SetupGet(x => x.gameItemId).Returns(gameItemId);
        contract.SetupGet(x => x.parameters).Returns(parameters);

        var moqObj = new Mock<IUObject>();
        _idObj.Add(gameItemId, moqObj.Object);

        _gameQueue.Add(gameId, new Queue<ICommand>());

        new RegistFireCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Command.Fire",
        (object[] args) =>
        {
            var cmdFire = new Mock<ICommand>();

            return cmdFire.Object;
        }
        ).Execute();

        var mh = new MessageHandler(contract.Object);
        mh.Execute();

        Assert.Single(_gameQueue[gameId]);
    }

    [Fact]
    public void CreateStartMoveCommandSuccesful()
    {
        var type = "StartMove";
        var gameId = "asdfg";
        var gameItemId = 548;
        var parameters = new Dictionary<string, object>();

        var contract = new Mock<IContract>();
        contract.SetupGet(x => x.type).Returns(type);
        contract.SetupGet(x => x.gameId).Returns(gameId);
        contract.SetupGet(x => x.gameItemId).Returns(gameItemId);
        contract.SetupGet(x => x.parameters).Returns(parameters);

        var moqObj = new Mock<IUObject>();
        _idObj.Add(gameItemId, moqObj.Object);

        _gameQueue.Add(gameId, new Queue<ICommand>());

        new RegistStartMoveCommand().Execute();

        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Command.StartMove",
        (object[] args) =>
        {
            var cmdStartMove = new Mock<ICommand>();

            return cmdStartMove.Object;
        }
        ).Execute();

        var mh = new MessageHandler(contract.Object);
        mh.Execute();

        Assert.Single(_gameQueue[gameId]);
    }

    [Fact]
    public void CreateEndMoveCommandSuccesful()
    {
        var type = "EndMove";
        var gameId = "asdfg";
        var gameItemId = 548;
        var parameters = new Dictionary<string, object>() { { "command to end", "Rotate" } };

        var contract = new Mock<IContract>();
        contract.SetupGet(x => x.type).Returns(type);
        contract.SetupGet(x => x.gameId).Returns(gameId);
        contract.SetupGet(x => x.gameItemId).Returns(gameItemId);
        contract.SetupGet(x => x.parameters).Returns(parameters);

        var moqObj = new Mock<IUObject>();
        _idObj.Add(gameItemId, moqObj.Object);

        _gameQueue.Add(gameId, new Queue<ICommand>());

        new RegistEndMoveCommand().Execute();

        var endable = new Mock<IMoveCommandEndable>();
        endable.Setup(x => x.Object).Returns(moqObj.Object);

        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Command.EndMove",
        (object[] args) =>
        {
            var obj = (IUObject)args[0];
            var param = (IDictionary<string, object>)args[1];

            endable.Setup(x => x.Command).Returns((string)param["command to end"]);
            endable.Setup(x => x.Property).Returns(new List<string> { "Angle" });

            var cmdEndMove = new EndMoveCommand(endable.Object);

            return cmdEndMove;
        }
        ).Execute();

        var mh = new MessageHandler(contract.Object);
        mh.Execute();

        var cmd = _gameQueue[gameId].Dequeue();

        Assert.Equal(typeof(EndMoveCommand), cmd.GetType());
    }
}
