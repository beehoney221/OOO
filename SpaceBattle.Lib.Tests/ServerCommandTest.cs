using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Tests;
public class ServerCommandTest
{
    public ServerCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Thread.SendCommand",
            (object[] args) =>
            {
                var id = (int)args[0];
                var cmd = (ICommand)args[1];

                return cmd;
            }
        ).Execute();

        new StartServerCommand().Execute();
        new StopServerCommand().Execute();
    }

    [Fact]
    public void ServerStartSuccesful()
    {
        var num = 5;

        var moqCmdStart = new Mock<ICommand>();
        moqCmdStart.Setup(c => c.Execute()).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Thread.CreateAndStart",
            (object[] args) =>
            {
                return moqCmdStart.Object;
            }
        ).Execute();

        IoC.Resolve<ICommand>("Server.Start.Cmd", num).Execute();

        moqCmdStart.Verify(c => c.Execute(), Times.Exactly(num));
    }

    [Fact]
    public void ServerStopSuccesful()
    {
        var num = 5;

        var moqCmdStop = new Mock<ICommand>();
        moqCmdStop.Setup(c => c.Execute()).Verifiable();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Thread.SoftStop",
            (object[] args) =>
            {
                var act = (Action)args[1];
                var th = new Thread(() => act());

                th.Start();

                return moqCmdStop.Object;
            }
        ).Execute();

        IoC.Resolve<ICommand>("Server.Stop.Cmd", num).Execute();

        moqCmdStop.Verify(c => c.Execute(), Times.Exactly(num));
    }

    [Fact]
    public void LogStrategySuccesful()
    {
        var exc = new Exception("Threads' transmitted and expected ID are not coincides.");
        var path = Path.GetTempFileName();

        new StrategyLog(path).Execute();

        IoC.Resolve<ICommand>("Exception.Log", new Mock<ICommand>().Object, exc).Execute();

        var sr = new StreamReader(path);
        var str = sr.ReadLine()?.Split(" - ")[1];
        sr.Close();
        File.Delete(path);

        Assert.Equal(exc.Message, str);
    }
}
