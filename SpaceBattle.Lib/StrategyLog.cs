using Hwdtech;

namespace SpaceBattle.Lib;

public class StrategyLog : ICommand
{
    private readonly string _path;
    public StrategyLog(string path)
    {
        _path = path;
    }
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Exception.Log",
        (object[] args) =>
        {
            var cmd = (ICommand)args[0];
            var exc = (Exception)args[1];

            var result = new ActionCommand(() =>
            {
                var sw = new StreamWriter(_path);
                sw.WriteLine($"{DateTime.Now} - {exc.Message}");
                sw.Close();
            });

            return result;
        }
        ).Execute();
    }
}
