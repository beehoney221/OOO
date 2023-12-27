using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Tests;
public class LongOperationTest
{
    private readonly Mock<ICommand> _moqCmd;
    public LongOperationTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        _moqCmd = new Mock<Lib.ICommand>();

        var moqResolve = new Mock<Func<object[], object>>();
        moqResolve.Setup(x => x(It.IsAny<object[]>())).Returns(_moqCmd.Object);

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "MacroCmd",
            (object[] args) =>
            {
                return _moqCmd.Object;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Cmd.LongOperation",
            (object[] args) =>
            {
                return _moqCmd.Object;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "Q.LongOperation",
            (object[] args) =>
            {
                return _moqCmd.Object;
            }
        ).Execute();
    }

    [Fact]
    public void LongOperationTestDependCmd()
    {
        var depend = "Dependency";
        var moqObj = new Mock<IUObject>();

        _moqCmd.Setup(m => m.Execute()).Verifiable();

        var longOperation = new LongOperation(depend, moqObj.Object);

        longOperation.Execute();
        _moqCmd.Verify(m => m.Execute(), Times.Exactly(3));
    }
}
