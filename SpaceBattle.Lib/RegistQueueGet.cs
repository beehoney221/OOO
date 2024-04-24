using Hwdtech;

namespace SpaceBattle.Lib;

public class RegistQueueGet : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Queue.Get",
        (object[] args) =>
        {
            var gameId = (string)args[0];
            var gameQueue = IoC.Resolve<Dictionary<string, Queue<ICommand>>>("Get.GameQueue");
            
            return gameQueue[gameId];
        }
        ).Execute();
    }
}
