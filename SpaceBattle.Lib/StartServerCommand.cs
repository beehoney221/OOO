using Hwdtech;

namespace SpaceBattle.Lib;

public class StartServerCommand : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Server.Start.Cmd",
        (object[] args) =>
        {
            var numThreads = (int)args[0];
            var i = 0;

            var cmd = new ActionCommand(() =>
            {
                while (i < numThreads)
                {
                    IoC.Resolve<ICommand>("ServerThread.CreateAndStart", i, () => { /*bar.SignalAndWait();*/ }).Execute();
                    i++;
                }
                // bar.SignalAndWait();
            });

            return cmd;
        }
        ).Execute();
    }
}
