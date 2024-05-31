using Hwdtech;

namespace SpaceBattle.Lib;

public class RegistGameDelete : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Delete",
            (object[] args) =>
            {
                var gameId = (string)args[0];

                var gameScopes = IoC.Resolve<Dictionary<string, object>>("Get.GameScopes");
                var gameCommandsQueue = IoC.Resolve<Dictionary<string, Queue<ICommand>>>("Get.GameQueue");
                
                if (gameScopes.ContainsKey(gameId) & gameCommandsQueue.ContainsKey(gameId)) 
                {
                    var deleteGameCommand = new ActionCommand(() =>
                    {
                        gameScopes.Remove(gameId);
                        gameCommandsQueue.Remove(gameId);
                    });

                    return deleteGameCommand;
                }
                else 
                { 
                    throw new Exception(); 
                }
            }
        ).Execute();
    }
}
