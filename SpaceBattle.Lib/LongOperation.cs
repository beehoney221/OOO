using Hwdtech;

namespace SpaceBattle.Lib;

public class LongOperation : ICommand
{
    private readonly string _name;
    private readonly IUObject _obj;

    public LongOperation(string name, IUObject obj)
    {
        _name = name;
        _obj = obj;
    }

    public void Execute()
    {
        var macroCmd = IoC.Resolve<ICommand>("MacroCmd", _name, _obj);
        macroCmd.Execute();

        var repCmd = IoC.Resolve<ICommand>("Cmd.LongOperation", macroCmd);
        repCmd.Execute();

        var qCmd = IoC.Resolve<ICommand>("Q.LongOperation", repCmd);
        qCmd.Execute();
    }
}