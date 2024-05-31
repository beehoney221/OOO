using Hwdtech;

namespace SpaceBattle.Lib;

public class RegistScopeCreate : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.Scope.Create",
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
