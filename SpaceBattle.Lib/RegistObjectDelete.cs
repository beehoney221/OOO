using Hwdtech;

namespace SpaceBattle.Lib;

public class RegistObjectDelete : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Object.Delete",
        (object[] args) =>
        {
            var gameItemId = (int)args[0];
            var idObject = IoC.Resolve<Dictionary<int, IUObject>>("Get.IdObject");

            if (idObject.ContainsKey(gameItemId)) 
            {
                return new ActionCommand(() => idObject.Remove(gameItemId));
            }
            else 
            { 
                throw new Exception(); 
            }
        }
        ).Execute();
    }
}
