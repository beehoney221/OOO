using Hwdtech;
using Hwdtech.Ioc;
using Moq;

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
            var shipPosition = new Vector(new int[] { 10, 20 });
            var shipVelocity = new Vector(new int[] { 1, 2 });
            var mockShip = new Mock<IMovable>();
            mockShip.SetupGet(s => s.Position).Returns(shipPosition);
            mockShip.SetupGet(s => s.Velocity).Returns(shipVelocity);

            var startMoveCommandMock = new Mock<ICommand>();

            IoC.Resolve<Hwdtech.ICommand>("IoC.Register", "Game.StartMoveCommand", (object[] args) =>
            {
                return startMoveCommandMock.Object;
            }).Execute();

            var shootCommand = new ShootCommand(mockShip.Object);

            shootCommand.Execute();

            startMoveCommandMock.Verify(cmd => cmd.Execute(), Times.Once);
        }

        [Fact]
        public void Torpedo_SetValues()
        {
            var position = new Vector(new int[] { 0, 0 });
            var velocity = new Vector(new int[] { 1, 1 });

            var torpedo = new Torpedo(position, velocity);

            Assert.Equal(position, torpedo.Position);
            Assert.Equal(velocity, torpedo.Velocity);
        }

        [Fact]
        public void Torpedo_Position_CanBeModified()
        {
            var initialPosition = new Vector(new int[] { 0, 0 });
            var newPosition = new Vector(new int[] { 1, 1 });
            var velocity = new Vector(new int[] { 0, 0 });
            var torpedo = new Torpedo(initialPosition, velocity);

            torpedo.Position = newPosition;

            Assert.Equal(newPosition, torpedo.Position);
        }
    }
}
