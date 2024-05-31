using Hwdtech;

namespace SpaceBattle.Lib;

public class RegistGameCreate : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Create",
            (object[] args) =>
            {
                var gameId = (string)args[0];

                var gameCommand = new Queue<ICommand>();
                var gameCommandsQueue = IoC.Resolve<Dictionary<string, Queue<ICommand>>>("Get.GameQueue");
                gameCommandsQueue.Add(gameId, gameCommand);

                var gameCommandScope = IoC.Resolve<object>("Game.Scope.Create", args);
                var gameScopes = IoC.Resolve<Dictionary<string, object>>("Get.GameScopes");
                gameScopes.Add(gameId, gameCommandScope);

                return gameCommand;
            }
        ).Execute();
    }
}
