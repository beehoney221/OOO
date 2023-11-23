namespace SpaceBattle.Lib.Tests;
using Moq;

public class MoveCommandTest
{
    [Fact]
    public void MoveCommandPositive()
    {
        //pre

        var movable = new Mock<IMovable>();
        movable.SetupGet(m => m.Position).Returns(new Vector(new int[] { 12, 5 })).Verifiable();
        movable.SetupGet(m => m.Velocity).Returns(new Vector(new int[] { 12, 5 })).Verifiable();
        var mc = new MoveCommand(movable.Object);

        //act
        mc.Execute();

        //post
        //movable // pos == (7, 8)

        movable.VerifySet(m => m.Position = new Vector(new int[] { 24, 10 }), Times.Once);
        movable.VerifyAll();
    }
}
