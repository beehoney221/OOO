using Hwdtech;

using System.Collections.Concurrent;

public class ServerThread {
    private readonly BlockingCollection<ICommand> _q;
    private Action _behaviour;
    private Thread _t;
    private bool _stop = false;

    public ServerThread(BlockingCollection<ICommand> q)
    {
        _q = q;

        _behaviour = () =>
        {
            var cmd = _q.Take();
            try
            {
                cmd.Execute();
            }
            catch (Exception e) {
                IoC.Resolve<ICommand>("ExceptionHandler.Handle", cmd, e).Execute();
            }
        };

        _t = new Thread(()=> {
            while(!_stop) {
                _behaviour();
            }
        });
    }

    public void Start() {
        _t.Start();
    }

    internal void Stop() {
        _stop = true;
    }

    internal void UpdateBehaviour(Action behaviour) {
        _behaviour = behaviour;
    }
}

public class HardStopCommand : ICommand
{
    private ServerThread _st;
    public HardStopCommand(ServerThread st) {
        _st = st;
    }
    public void Execute()
    {
        // IoC.Resolve<ICommand>(
        // )
        _st.Stop();
    }
}

// public class SoftStop: ICommand
// {
//     private ServerThread _st;
//     public SoftStop(ServerThread st){
//         _st = st;
//     }
//     public void Execute()
//     {
        
//     }
// }
