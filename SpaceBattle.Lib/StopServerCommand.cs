using Hwdtech;

namespace SpaceBattle.Lib;

public class StopServerCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Server.Stop.Cmd",
        (object[] args) =>
        {
            var numThreads = (int)args[0];
            // var bar = new Barrier(numThreads + 1);
            var i = 0;

            var cmd = new ActionCommand(() =>
            {
                while (i < numThreads)
                {
                    IoC.Resolve<ICommand>("ServerThread.SoftStop", i, () => { /* bar.SignalAndWait(); */ }).Execute();
                    i++;
                }
                // bar.SignalAndWait();
            });

            return cmd;
        }
        ).Execute();
    }
}
