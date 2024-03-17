using Moq;

using System.Collections.Concurrent;

using Hwdtech;
using Hwdtech.Ioc;

public class ServerThreadTest
{
    public ServerThreadTest() {

        new InitScopeBasedIoCImplementationCommand().Execute();

        IoC.Resolve<ICommand>("IoC.Register", 
            "Hard Stop The Thread",
            (object[] args) => {
                var thread = (ServerThread)args[0];
                var action = (Action)args[1]; 
                return new ActionCommand (
                    () => {
                        new HardStopCommand(thread).Execute();
                        new ActionCommand(action).Execute();
                    }
                );
            }
        ).Execute();

    }
    [Fact]
    public void HardStopShouldStopServerThread()
    {
        // AAA ~ {P}, S, {Q}
        // Arrange

        // Create Queue
        var q = new BlockingCollection<ICommand>(10);
        // Create ST
        var st = new ServerThread(q);

        // fill Queue: [c, HS, c]
        var command = new Mock<ICommand>();
        command.Setup(m => m.Execute()).Verifiable();

        q.Add(command.Object);
        var mre = new ManualResetEvent(false);
        q.Add(IoC.Resolve<ICommand>("Hard Stop The Thread", st, ()=> {mre.Set();}));
        q.Add(command.Object);
        

        // Act
        // ST.Start()
        st.Start();
        mre.WaitOne();

        // Assert
        // q.count == 1;
        Assert.Single(q);
        command.Verify(m=> m.Execute(), Times.Once);
    }
}

public class ActionCommand: ICommand {
    private readonly Action _action;
    public ActionCommand(Action action) {
        _action = action;
    }

    public void Execute()
    {
        _action();
    }
}
