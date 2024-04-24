namespace SpaceBattle.Lib;

public interface IContract
{
    public string type { get; }
    public string gameId { get; }
    public int gameItemId { get; }
    public IDictionary<string, object> parameters { get; }
}
