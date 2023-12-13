using Moq;
namespace SpaceBattle.Lib.Tests;

public class RotateTest
{
    [Fact]
    public void RotateCommandPositive()
    {

        var rotatable = new Mock<IRotatable>();
        rotatable.SetupGet(m => m.Angle).Returns(new Angle(45)).Verifiable();
        rotatable.SetupGet(m => m.AngleVelocity).Returns(new Angle(45)).Verifiable();
        var rc = new RotateCommand(rotatable.Object);

        rc.Execute();

        rotatable.VerifySet(m => m.Angle = It.Is<Angle>(e => e.tilt == 90));
        rotatable.VerifyAll();
    }
}
