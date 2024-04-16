using Hwdtech;

namespace SpaceBattle.Lib;

public class MessageHandler : ICommand
{
    private Queue<Contract>? _q;
    public void PutQ(Contract contr)
    {
        var q = new Queue<Contract>();
        q.Enqueue(contr);
        _q = q;
    }
    public void Execute()
    {
        var contract = _q.Peek();
        // извлечь из очереди сообщение

        new RegistInterpretCommand().Execute();

        var cmd = IoC.Resolve<ICommand>("InterpretCommand", contract); // создание команды интерпретации сообщения
        
        cmd.Execute(); // execute : интерпретация сообщения --> создание команды --> появление команды в очереди соответствующей игры
    }
}

public class Contract
{
    public string? cmdType { get; set;}
    public IDictionary<string, object>? parameters;
}