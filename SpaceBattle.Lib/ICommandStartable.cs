namespace SpaceBattle.Lib;

public interface ICommandStartable
{
    IUObject uobject {get;}
    IDictionary<string, object> Properties {get;}
}
