using Hwdtech;

namespace SpaceBattle.Lib;

public class ConsoleApp
{
    public static void Main(string[] args)
    {
        new RegistStartServerCommand().Execute();
        new RegistStopServerCommand().Execute();

        var num = Convert.ToInt32(args[0]);
        Console.WriteLine("Процедура запуска сервера началась.");
        IoC.Resolve<ICommand>("Server.Start.Cmd", num).Execute();
        Console.WriteLine("Сервер успешно запущен.");
        Console.ReadLine();
        Console.WriteLine("Процедура остановки сервера началась.");
        IoC.Resolve<ICommand>("Server.Stop.Cmd", num).Execute();
        Console.WriteLine("Сервер успешно остановлен.");
    }
}
