namespace SpaceBattle.Lib;

public class Angle
{
    public int tilt { get; set; }
    private int var { get; } = 360;

    public Angle(int r)
    {
        tilt = r;
    }

    public static Angle operator +(Angle angle_1, Angle angle_2)
    {
        return new Angle((angle_1.tilt + angle_2.tilt) % angle_1.var);
    }
}
