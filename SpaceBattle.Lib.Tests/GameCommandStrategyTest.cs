using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Tests;
public class GameCommandStrategyTest
{
    public Dictionary<string, object> _gameScopes = new();
    public Dictionary<string, Queue<ICommand>> _gameCommandsQueue = new();
    public Dictionary<string, Dictionary<int, IUObject>> _gameIdObject = new();
    public GameCommandStrategyTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Get.GameQueue",
            (object[] args) =>
            {
                return _gameCommandsQueue;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Get.GameIdObject",
            (object[] args) =>
            {
                return _gameIdObject;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Get.GameScopes",
            (object[] args) =>
            {
                return _gameScopes;
            }
        ).Execute();
    }

    [Fact]
    public void GameCommandStrategySuccesful()
    {
        var gameId = "asdfg";
        var quant = 5;
        var prntScope = IoC.Resolve<object>("Scopes.Current");
        var gameItemId = 548;

        var IdObj = new Dictionary<int, IUObject>();
        var obj = new Mock<IUObject>();

        IdObj.Add(gameItemId, obj.Object);
        _gameIdObject.Add("asdfg", IdObj);

        var cmd = new Mock<ICommand>();

        new RegistScopeCreate().Execute();
        new RegistGameCreate().Execute();

        IoC.Resolve<Queue<ICommand>>("Game.Create", gameId, quant, prntScope);

        Assert.Single(_gameCommandsQueue);
        Assert.Single(_gameScopes);

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", _gameScopes[gameId]).Execute();

        Assert.Equal(quant, int.Parse(IoC.Resolve<string>("Game.Quantum.Get")));

        IoC.Resolve<ICommand>("Game.Queue.Put", cmd.Object).Execute();
        Assert.Equal(cmd.Object, IoC.Resolve<ICommand>("Game.Queue.Pull"));

        Assert.Equal(obj.Object, IoC.Resolve<IUObject>("Game.Object.Get", gameItemId));
        IoC.Resolve<ICommand>("Game.Object.Delete", gameItemId).Execute();
        Assert.False(_gameIdObject[gameId].ContainsKey(gameItemId));

        new RegistGameDelete().Execute();

        IoC.Resolve<ICommand>("Game.Delete", gameId).Execute();
        Assert.Empty(_gameCommandsQueue);
        Assert.Empty(_gameScopes);
    }

    [Fact]
    public void GameCommandWithTheSameIdHasAlreadyBeenAdded()
    {
        var gameId = "asdfg";
        var quant = 5;
        var prntScope = IoC.Resolve<object>("Scopes.Current");

        new RegistScopeCreate().Execute();
        new RegistGameCreate().Execute();

        IoC.Resolve<Queue<ICommand>>("Game.Create", gameId, quant, prntScope);

        var gameCreate = () => IoC.Resolve<Queue<ICommand>>("Game.Create", gameId, quant, prntScope);
        Assert.Throws<ArgumentException>(gameCreate);
    }

    [Fact]
    public void ParentScopeNotDefined()
    {
        var gameId = "asdfg";
        var quant = 5;

        new RegistScopeCreate().Execute();
        new RegistGameCreate().Execute();

        var scopeCreate = () => IoC.Resolve<Queue<ICommand>>("Game.Create", gameId, quant);

        Assert.ThrowsAny<Exception>(scopeCreate);
    }

    [Fact]
    public void QuantumNotDefined()
    {
        var gameId = "asdfg";
        var prntScope = IoC.Resolve<object>("Scopes.Current");

        new RegistScopeCreate().Execute();
        new RegistGameCreate().Execute();

        var scopeCreate = () => IoC.Resolve<Queue<ICommand>>("Game.Create", gameId, prntScope);
        Assert.ThrowsAny<Exception>(scopeCreate);
    }

    [Fact]
    public void GameNotFound()
    {
        var gameId = "asdfg";
        var prntScope = IoC.Resolve<object>("Scopes.Current");

        new RegistGameDelete().Execute();

        var queuePut = () => IoC.Resolve<ICommand>("Game.Queue.Put", gameId);
        var gameDelete = () => IoC.Resolve<ICommand>("Game.Delete", gameId);

        Assert.ThrowsAny<Exception>(queuePut);
        Assert.ThrowsAny<Exception>(gameDelete);
    }

    [Fact]
    public void GameQueueEmpty()
    {
        var gameId = "asdfg";
        var quant = 5;
        var prntScope = IoC.Resolve<object>("Scopes.Current");

        new RegistScopeCreate().Execute();
        new RegistGameCreate().Execute();

        IoC.Resolve<Queue<ICommand>>("Game.Create", gameId, quant, prntScope);

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", _gameScopes[gameId]).Execute();

        var queuePull = () => IoC.Resolve<ICommand>("Game.Queue.Pull", gameId);
        Assert.ThrowsAny<Exception>(queuePull);
    }

    [Fact]
    public void GameObjectWithThisIdNotFound()
    {
        var gameId = "asdfg";
        var quant = 5;
        var prntScope = IoC.Resolve<object>("Scopes.Current");
        var gameItemId = 548;

        var IdObj = new Dictionary<int, IUObject>();

        _gameIdObject.Add("asdfg", IdObj);

        new RegistScopeCreate().Execute();
        new RegistGameCreate().Execute();
        IoC.Resolve<Queue<ICommand>>("Game.Create", gameId, quant, prntScope);

        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", _gameScopes[gameId]).Execute();

        var objGet = () => IoC.Resolve<IUObject>("Game.Object.Get", gameItemId);
        var objDelete = () => IoC.Resolve<ICommand>("Game.Object.Delete", gameItemId);
        Assert.ThrowsAny<Exception>(objGet);
        Assert.ThrowsAny<Exception>(objDelete);
    }
}
