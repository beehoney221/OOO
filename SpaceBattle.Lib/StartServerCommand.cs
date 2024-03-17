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
            
            var threadsId = Enumerable.Range(0, numThreads).ToArray();
            var cmd = new ActionCommand(() =>
            {
                Array.ForEach(threadsId, i => { 
                    IoC.Resolve<ICommand>("Thread.CreateAndStart", i, () => {}).Execute(); });
            });

            return cmd;
        }
        ).Execute();
    }
}
