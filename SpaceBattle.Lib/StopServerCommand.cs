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
            var bar = new Barrier(numThreads + 1);
            
            var threadsId = Enumerable.Range(0, numThreads).ToArray();
            var cmd = new ActionCommand(() =>
            {
                Array.ForEach(threadsId, i => { 
                        IoC.Resolve<ICommand>(
                            "Thread.SendCommand", i,
                            IoC.Resolve<ICommand>("Thread.SoftStop", i, () => { bar.SignalAndWait(); })).Execute();});
            });
            
            return cmd;
        }
        ).Execute();
    }
}
