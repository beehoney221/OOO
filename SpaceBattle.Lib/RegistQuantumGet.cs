using Hwdtech;

namespace SpaceBattle.Lib;

public class RegistQuantumGet : ICommand
{
    private readonly int _quant;
    public RegistQuantumGet(int quant)
    {
        _quant = quant;
    }
    public void Execute()
    {
        IoC.Resolve<Hwdtech.ICommand>(
        "IoC.Register",
        "Game.Quantum.Get",
        (object[] args) =>
        {
            return Convert.ToString(_quant);
        }
        ).Execute();
    }
}