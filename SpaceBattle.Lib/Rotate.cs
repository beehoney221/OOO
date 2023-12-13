namespace SpaceBattle.Lib;

public interface IRotatable
{
    public Angle Angle { get; set; }
    public Angle AngleVelocity { get; }
}

public class RotateCommand : ICommand
{
    private readonly IRotatable rotatable;

    public RotateCommand(IRotatable rotatable)
    {
        this.rotatable = rotatable;
    }

    public void Execute()
    {
        rotatable.Angle = rotatable.Angle + rotatable.AngleVelocity;
    }
}
