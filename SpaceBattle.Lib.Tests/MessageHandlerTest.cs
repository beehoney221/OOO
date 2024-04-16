using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Tests;
public class MessageHandlerTest
{
    public MessageHandlerTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "CreateCommand",
            (object[] args) =>
            {
                var contract = (Contract)args[0];
                var moqCmd = new Mock<ICommand>();

                IoC.Resolve<Hwdtech.ICommand>(
                    "IoC.Register",
                    $"Command.{contract.cmdType}",
                    (object[] args) =>
                    {
                        var param = contract.parameters;

                        return moqCmd.Object; //// как передать параметры моковой команде
                    }
                ).Execute();
                
                return moqCmd.Object;
            }
        ).Execute();
    }

    [Fact]
    public void HandlerSuccesful()
    {
        var moqCmdStart = new Mock<ICommand>();
        moqCmdStart.Setup(c => c.Execute()).Verifiable();
        
        var mh = new MessageHandler();
        
        var contract = new Contract();
        contract.cmdType = "fire";
        contract.parameters = new Dictionary<string, object>() { {"game id", "asdfg"}, {"game item id", 548}};

        mh.PutQ(contract);
        mh.Execute();

        moqCmdStart.Verify(c => c.Execute(), Times.Exactly(5));
    }
}
