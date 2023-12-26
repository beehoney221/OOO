namespace SpaceBattle.Lib;

public interface IQueue
{
    public void Add(ICommand cmd);
    public ICommand Take();
}
