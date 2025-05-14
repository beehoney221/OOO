using Moq;
using Hwdtech;
using Hwdtech.Ioc;
using SpaceBattle.Lib;

namespace SpaceBattle.Lib.Tests
{
    public class ShootCommandTests
    {
        public ShootCommandTests()
        {
            new InitScopeBasedIoCImplementationCommand().Execute();
            IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set",
                IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();
        }
         
        [Fact]
        public void ShootCommand_Positive()
        {
            var torpedoPosition = new Vector(new int[] { 10, 20 });
            var torpedoVelocity = new Vector(new int[] { 1, 0 });

            var mockTorpedo = new Mock<IUObject>();
            var mockShootableSource = new Mock<IUObject>();
            var mockStartMoveCommand = new Mock<ICommand>();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.Get.Property",
                (object[] args) => ((IUObject)args[0]).GetProperty((string)args[1])
            ).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.Set.Property",
                (object[] args) => 
                {
                    var obj = (IUObject)args[0];
                    var prop = (string)args[1];
                    var value = args[2];
                    return new ActionCommand(() => obj.SetProperty(prop, value));
                }
            ).Execute();

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.New.UObject",
                (object[] args) => mockTorpedo.Object
            ).Execute();

            new CreatTorpedo().Create(new Mock<IShootable>().Object);

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.Command.StartMove",
                (object[] args) => mockStartMoveCommand.Object
            ).Execute();

            mockShootableSource.Setup(o => o.GetProperty("Position")).Returns(torpedoPosition);
            mockShootableSource.Setup(o => o.GetProperty("Velocity")).Returns(torpedoVelocity);

            var adapter = new IShootableAdapter(mockShootableSource.Object);
            Assert.Equal(torpedoPosition, adapter.torpedoPosition);
            Assert.Equal(torpedoVelocity, adapter.torpedoVelocity);

            var shootCommand = new ShootCommand(adapter);
            shootCommand.Execute();

            mockTorpedo.Verify(o => o.SetProperty("Position", torpedoPosition), Times.Once);
            mockTorpedo.Verify(o => o.SetProperty("Velocity", torpedoVelocity), Times.Once);

            mockStartMoveCommand.Verify(cmd => cmd.Execute(), Times.Once);
        }

        [Fact]
        public void TorpedoPosition_Set()
        {
            var mockUObject = new Mock<IUObject>();
            var newPosition = new Vector(new int[] { 30, 40 });

            IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.Set.Property",
                (object[] args) => 
                {
                    var obj = (IUObject)args[0];
                    var prop = (string)args[1];
                    var value = args[2];
                    return new ActionCommand(() => obj.SetProperty(prop, value));
                }
            ).Execute();

            var adapter = new IShootableAdapter(mockUObject.Object);

            adapter.torpedoPosition = newPosition;

            mockUObject.Verify(o => o.SetProperty("Position", newPosition), Times.Once);
        }
    }
}
