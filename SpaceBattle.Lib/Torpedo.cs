using SpaceBattle.Lib;

namespace SpaceBattle;
public class Torpedo : IMovable
{
    public Vector Position { get; set; }
    public Vector Velocity { get; }

    public Torpedo(Vector position, Vector velocity)
    {
        Position = position;
        Velocity = velocity;
    }
}
