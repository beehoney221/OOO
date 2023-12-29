using Hwdtech;
namespace SpaceBattle.Lib;
public class MacroCommandBuild : ICommand
{
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.MacroCommand.Build",
        (object[] args) =>
        {   
            var dependency = (string)args[0];
            var cmdNames = IoC.Resolve<string[]>(dependency);

            var cmds = new List<ICommand>();

            cmdNames.ToList().ForEach(cmd_name =>
            {
                cmds.Add(IoC.Resolve<ICommand>(cmd_name)); 
            });

            var macroCommand = new MacroCommand(cmds);
            return macroCommand;
        }
        ).Execute();
    }
}