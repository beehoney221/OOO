using Hwdtech;

namespace SpaceBattle.Lib;

public class StopServerCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Server.Stop.Cmd",
        (int numThreads) =>
        {
            var i = 1;
            Console.Write("Началась процедура остановки.");
            while (i <= numThreads)
            {

            }

            var barrier = new Barrier(numThreads);
            return "\nПроцедура остановки завершена!";
        }
        ).Execute();
    }
}
