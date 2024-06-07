using Hwdtech;

namespace SpaceBattle.Lib;

public class RegistStartMoveCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Command.StartMove",
            (object[] args) =>
            {
                var obj = (IUObject)args[0];
                

                var gameScope = IoC.Resolve<object>("Scopes.New", prntScope);

                IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", gameScope).Execute();

                IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Get.IdObject",
                (object[] args) =>
                {
                    var gameIdObject = IoC.Resolve<Dictionary<string, Dictionary<int, IUObject>>>("Get.GameIdObject");

                    return gameIdObject[gameId];
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
    }
}
