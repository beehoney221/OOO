using Hwdtech;

public class MacroCommandBuild
{
    private readonly string _dependencyName;

    public MacroCommandBuild(string dependencyName)
    {
        _dependencyName = dependencyName;
    }

    public ICommand[] BuildCommand()
    {
        var cmds = new List<ICommand>();
        var cmdNames = IoC.Resolve<string[]>(_dependencyName);

        cmdNames.ToList().ForEach(cmd_name =>
        {
            cmds.Add(IoC.Resolve<ICommand>(cmd_name));
        });

        return cmds.ToArray();
    }
}