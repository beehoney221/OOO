using Hwdtech;

namespace SpaceBattle.Lib;

public class BuildSolTree : ICommand
{
    private readonly IRead _read;

    public BuildSolTree(IRead read)
    {
        _read = read;
    }

    public void Execute()
    {
        var vector = _read.ReadFile();
        var solTree = IoC.Resolve<Dictionary<int, object>>("BuildTree");

        vector.ForEach(
            line =>
            {
                line.ToList().ForEach(coord =>
                {
                    solTree.TryAdd(coord, new Dictionary<int, object>());
                    solTree = (Dictionary<int, object>)solTree[coord];
                }
                );
            }
        );
    }
}
