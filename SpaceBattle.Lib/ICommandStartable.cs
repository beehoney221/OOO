namespace SpaceBattle.Lib;

public interface ICommandStartable
{
    IUObject Target { get; }
    IDictionary<string, object> Properties { get; }
}
