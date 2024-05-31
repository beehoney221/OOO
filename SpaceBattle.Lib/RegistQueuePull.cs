using Hwdtech;

namespace SpaceBattle.Lib;

public class RegistQueuePull : ICommand
{
    private readonly string _gameId;
    public RegistQueuePull(string gameId)
    {
        _gameId = gameId;
    }
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Queue.Pull",
        (object[] args) =>
        {
            var gameQueue = IoC.Resolve<Queue<ICommand>>("Game.Queue.Get", _gameId);
            var cmd = gameQueue.Dequeue();

            return cmd;
        }
        ).Execute();
    }
}
