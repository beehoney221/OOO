using System.Diagnostics;
using Hwdtech;

namespace SpaceBattle.Lib;
public class GameCommand: ICommand {
    private readonly Queue<ICommand> _q;
    private readonly object _scope;
    public GameCommand(Queue<ICommand> q, object scope) {
        _q = q;
        _scope = scope;
    }
    public void Execute() {
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", _scope).Execute();
        var timeQuant = IoC.Resolve<long>("Game.TimeQuant");
        var sw = new Stopwatch();
        while (_q.Count > 0 && timeQuant >= 0) 
        {
            sw.Start();
            var cmd = _q.Dequeue();
            try 
            {
                cmd.Execute();
            } 
            
            catch (Exception e) 
            {
                IoC.Resolve<ICommand>("ExceptionHandler.Handle", cmd, e).Execute();
            } 

            sw.Stop();
            timeQuant -= sw.ElapsedMilliseconds;
            sw.Reset();
        }
    }
}