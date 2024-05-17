using Hwdtech;

namespace SpaceBattle.Lib;

public class RegistObjectGet : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Object.Get",
        (object[] args) =>
        {
            var gameItemId = (int)args[0];
            var idObj = IoC.Resolve<Dictionary<int, IUObject>>("Get.IdObject");
            
            return idObj[gameItemId];
        }
        ).Execute();
    }
}
