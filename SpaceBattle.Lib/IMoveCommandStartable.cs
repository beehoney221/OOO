namespace SpaceBattle.Lib;

public interface IMoveCommandStartable
{
    IUObject uobject {get;}
    Vector velocity {get;}
    IQueue<ICommand> queue {get;}
}
