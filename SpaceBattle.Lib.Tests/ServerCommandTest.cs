using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib.Tests;
public class ServerCommandTest
{
    private Dictionary<int, TestServerThread>? _dict;
    private ActionCommand? _actStop;
    public ServerCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var dict = new Dictionary<int, TestServerThread>();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "ServerThread.CreateAndStart",
            (object[] args) =>
            {
                var id = (int)args[0];
                var action = (Action)args[1];

                var actStart = new ActionCommand(() => {dict.Add(id, new TestServerThread());});

                _dict = dict;
                
                return actStart;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "ServerThread.SoftStop",
            (object[] args) =>
            {
                var id = (int)args[0];
                var action = (Action)args[1];

                var actStop = new ActionCommand(() => {dict[id].Stop();});
                _actStop = actStop;

                return _actStop;
            }
        ).Execute();
        
        new StartServerCommand().Execute();
        new StopServerCommand().Execute();
    }

    [Fact]
    public void ServerStartSuccesful()
    {
        var num = 5;

        IoC.Resolve<ICommand>("Server.Start.Cmd", num).Execute();

        Assert.Equal(num, _dict?.Count);
    }
    
    [Fact]
    public void ServerStopSuccesful()
    {
        var num = 5;

        IoC.Resolve<ICommand>("Server.Start.Cmd", num).Execute();
        IoC.Resolve<ICommand>("Server.Stop.Cmd", num).Execute();
        
        Assert.True(_dict?.Values.All(thread => thread.Status() == true));
    }
}

public class TestServerThread 
{
    private bool _stop = false;
    public bool Status()
    {
        return _stop;
    }
    public void Stop()
    {
        _stop = true;
    }
}
