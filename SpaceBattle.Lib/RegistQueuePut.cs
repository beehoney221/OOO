using Hwdtech;

namespace SpaceBattle.Lib;

public class RegistQueuePut : ICommand
{
    private readonly string _gameId;
    public RegistQueuePut(string gameId)
    {
        _gameId = gameId;
    }
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Queue.Put",
        (object[] args) =>
        {
            var gameQueue = IoC.Resolve<Queue<ICommand>>("Game.Queue.Get", _gameId);
            var cmd = (ICommand)args[0];

            return new ActionCommand(() => gameQueue.Enqueue(cmd));
        }
        ).Execute();
    }
}