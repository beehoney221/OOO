using Hwdtech;

namespace SpaceBattle.Lib;

public class StartServerCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Server.Start.Cmd",
        (int numThreads) =>
        {
            var i = 1;
            var barrier = new Barrier(numThreads);
            while (i <= numThreads)
            {
                var thread = new Thread(() => { });
                thread.Start();
                barrier.SignalAndWait();
            }

            barrier.SignalAndWait();
            return "\nУспешный запуск сервера!";
        }
        ).Execute();
    }
}
