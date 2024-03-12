using Hwdtech;
using Hwdtech.Ioc;

namespace SpaceBattle.Lib;

public class ConsoleApp
{
    public static void Main()
    {
        new InitScopeBasedIoCImplementationCommand().Execute();
        IoC.Resolve<Hwdtech.ICommand>("Scopes.Current.Set", IoC.Resolve<object>("Scopes.New", IoC.Resolve<object>("Scopes.Root"))).Execute();

        var dict = new Dictionary<int, TestServerThread>();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "ServerThread.CreateAndStart",
            (object[] args) =>
            {
                var id = (int)args[0];
                var action = (Action)args[1];

                var act = new ActionCommand(() =>
                {
                    Console.WriteLine(id);
                    dict.Add(id, new TestServerThread());
                });

                return act;
            }
        ).Execute();

        IoC.Resolve<Hwdtech.ICommand>(
            "IoC.Register",
            "ServerThread.SoftStop",
            (object[] args) =>
            {
                var id = (int)args[0];
                var action = (Action)args[1];

                var act = new ActionCommand(() => { dict[id].Stop(); });

                return act;
            }
        ).Execute();

        new StartServerCommand().Execute();
        new StopServerCommand().Execute();

        Console.WriteLine("Введите количество потоков: ");
        var num = Convert.ToInt32(Console.ReadLine());
        Console.WriteLine("Процедура запуска сервера началась.");
        IoC.Resolve<ICommand>("Server.Start.Cmd", num).Execute();
        Console.WriteLine("Сервер успешно запущен.");
        Console.ReadLine();
        Console.WriteLine("Процедура остановки сервера началась.");
        IoC.Resolve<ICommand>("Server.Stop.Cmd", num).Execute();
        Console.WriteLine("Сервер успешно остановлен.");
    }
}

public class TestServerThread
{
    private bool _stop = false;
    public bool Status()
    {
        return _stop;
    }
    public void Stop()
    {
        _stop = true;
    }
}
