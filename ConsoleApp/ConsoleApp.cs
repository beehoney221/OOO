using Hwdtech;

namespace ConsoleApp
{
    public class ConsoleApp
    {
        public static void Main()
        {
            Console.Write("Введите количество потоков: ");
            var num = Convert.ToInt32(Console.ReadLine());
            var start = IoC.Resolve<string>("Server.Start.Cmd", num);
            Console.WriteLine(start);
            Console.ReadLine();
            IoC.Resolve<string>("Server.Stop.Cmd", num);
        }
    }
}
