using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib;
public class GameCommandTest
{
    public GameCommandTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "ExceptionHandler.Handle",
            (object[] args) =>
            {

                return 0;
            }
        );

    }

    [Fact]
    public void GameCommandSuccesful()
    {
        var moqCmd = new Mock<ICommand>();
        moqCmd.Setup(c => c.Execute()).Verifiable();
        var scopes = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));

        var q = new Queue<ICommand>();
        q.Enqueue(moqCmd.Object);

        long quant = 0;

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.TimeQuant",
            (object[] args) =>
            {
                return (object)quant;
            }
        ).Execute();

        var gameCmd = new GameCommand(q, scopes);
        gameCmd.Execute();

        moqCmd.Verify(c => c.Execute(), Times.Exactly(1));
    }

    [Fact]
    public void GameCommandException()
    {
        var moqCmd = new Mock<ICommand>();
        moqCmd.Setup(c => c.Execute()).Throws(new Exception());
        var scopes = IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Current"));

        var q = new Queue<ICommand>();
        q.Enqueue(moqCmd.Object);

        long quant = 0;

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Game.TimeQuant",
            (object[] args) =>
            {
                return (object)quant;
            }
        ).Execute();

        var gameCmd = new GameCommand(q, scopes);
        gameCmd.Execute();

        moqCmd.Verify(c => c.Execute(), Times.Exactly(1));
    }
}
/*mre 
Sleep
или просто занулить квант времени
условные события
*/

