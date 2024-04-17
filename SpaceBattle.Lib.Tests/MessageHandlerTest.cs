using Hwdtech;
using Hwdtech.Ioc;
using Moq;

namespace SpaceBattle.Lib.Tests;
public class MessageHandlerTest
{
    public Mock<ICommand> _cmdAdd = new();
    public MessageHandlerTest()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var moqObj = new Mock<IUObject>();
        
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Object.Get",
        (object[] args) =>
        {
            var gameItemId = args[0];
            moqObj.Object.SetProperty("ID", gameItemId);

            return moqObj.Object;
        }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Command.Add",
        (object[] args) =>
        {
            var commandForQ = (ICommand)args[0];

            _cmdAdd.Setup(c => c.Execute()).Callback(() => commandForQ.Execute()).Verifiable();

            return _cmdAdd.Object;
        }
        ).Execute();

    }

    [Fact]
    public void HandlerSuccesful()
    {
        var contract = new Contract
        {
            type = "Rotate",
            gameId = "asdfg",
            gameItemId = 548,
            parameters = new Dictionary<string, object>(){{"angle velocity", 50}}
        };

        var rotatable = new Mock<IRotatable>();
        rotatable.SetupGet(x => x.Angle).Returns(new Angle(90));
        
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Command.Rotate",
        (object[] args) =>
        {
            var obj = (IUObject)args[0];
            var contract = (Contract)args[1];

            rotatable.SetupGet(x => x.AngleVelocity).Returns(new Angle((int)contract.parameters!["angle velocity"]));

            var cmdRotate = new RotateCommand(rotatable.Object);

            return cmdRotate;
        }
        ).Execute();
        
        var mh = new MessageHandler(contract);
        mh.Execute();

        _cmdAdd.Verify(c => c.Execute(), Times.Once());
    }
}
