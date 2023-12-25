namespace SpaceBattle.Lib;

public interface IQueue<T>
{
    void Add(ICommand cmd);
    ICommand Take();
}
