using Hwdtech;
using Scriban;

public class AdapterBuilder
{
    private readonly Template _templateCode;

    public AdapterBuilder()
    {
        _templateCode = Template.Parse(@"public class {{new_target_type}}Adapter : {{new_target_type}}
{
    readonly private {{target_type}} _obj;
    public {{new_target_type}}Adapter({{target_type}} obj) => _obj = obj;
{{for property in properties}}
    public {{property.property_type.name}} {{property.name}}
    {
{{if property.can_read}}
        get => IoC.Resolve<{{property.property_type.name}}>(""Game.Get.Property"", ""{{property.name}}"", _obj);
{{end}}
{{if property.can_write}}
        set => IoC.Resolve<ICommand>(""Game.Set.Property"", ""{{property.name}}"", _obj, value).Execute();
{{end}}
    }
{{end}}
}");
    }

    public void Builder()
    {

        IoC.Resolve<Hwdtech.ICommand>(
                "IoC.Register",
                "Game.Adapter.Build",
                (object[] args) =>
                {
                    var newTargetType = (Type)args[0];
                    var targetType = (Type)args[1];
                    var properties = newTargetType.GetProperties();
                    var result = _templateCode.Render(new
                    {
                        new_target_type = newTargetType.Name,
                        target_type = targetType.Name,
                        properties = properties

                    });
                    return result;
                }
            ).Execute();
    }
}
