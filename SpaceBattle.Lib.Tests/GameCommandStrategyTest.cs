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
            "Game.Scope.New",
            (object[] args) =>
            {
                var gameId = (string)args[0];
                var quant = (int)args[1];
                var prntScope = args[2];

                var gameScope = IoC.Resolve<object>("Scopes.New", prntScope);

                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", gameScope).Execute();

                IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Get.IdObject",
                (object[] args) =>
                {
                    return _gameIdObject[gameId];
                }
                ).Execute();

                new RegistQuantumGet(quant).Execute();

                new RegistQueueGet().Execute();

                new RegistQueuePut(gameId).Execute();

                new RegistQueuePull(gameId).Execute();

                new RegistObjectGet().Execute();

                new RegistObjectDelete().Execute();

                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", prntScope).Execute();

                return gameScope;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Create",
            (object[] args) =>
            {
                var gameId = (string)args[0];

                var gameCommand = new Queue<ICommand>();
                _gameCommandsQueue.Add(gameId, gameCommand);
                
                var gameCommandScope = IoC.Resolve<object>("Game.Scope.New", args);
                _gameScopes.Add(gameId, gameCommandScope);

                return gameCommand;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Delete",
            (object[] args) =>
            {
                var gameId = (string)args[0];

                var deleteGameCommand = new ActionCommand(() => {
                    _gameScopes.Remove(gameId);
                    _gameCommandsQueue.Remove(gameId);
                    });

                return deleteGameCommand;
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

        IoC.Resolve<Queue<ICommand>>("Game.Create", gameId, quant, prntScope);
        
        var gameCreate = () => IoC.Resolve<Queue<ICommand>>("Game.Create", gameId, quant, prntScope);
        Assert.Throws<ArgumentException>(gameCreate);
    }

    [Fact]
    public void ParentScopeNotDefined()
    {   
        var gameId = "asdfg";
        var quant = 5;

        var scopeCreate = () => IoC.Resolve<Queue<ICommand>>("Game.Create", gameId, quant);
        
        Assert.ThrowsAny<Exception>(scopeCreate);
    }

    [Fact]
    public void QuantumNotDefined()
    {   
        var gameId = "asdfg";
        var prntScope = IoC.Resolve<object>("Scopes.Current");

        var scopeCreate = () => IoC.Resolve<Queue<ICommand>>("Game.Create", gameId, prntScope);
        Assert.ThrowsAny<Exception>(scopeCreate);
    }
}