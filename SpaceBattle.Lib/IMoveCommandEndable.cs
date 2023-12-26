namespace SpaceBattle.Lib;

public interface IMoveCommandEndable
{
    public string Command { get; }
    public IUObject Object { get; }
    public IEnumerable<string> Property { get; }
}