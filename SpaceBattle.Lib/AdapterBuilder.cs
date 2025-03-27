using Hwdtech;
using Scriban;

public class AdapterBuilder
{
    private readonly Type _targetType;
    private readonly Type _newTargetType;

    public AdapterBuilder(Type targetType, Type newTargetType)
    {
        _targetType = targetType;
        _newTargetType = newTargetType;
    }

    public string Build()
    {
        var templateCode = IoC.Resolve<string>("Template");

        var template = Template.Parse(templateCode);

        var properties = _newTargetType.GetProperties()
            .Select(property => new
            {
                name = property.Name,
                property_type = new
                {
                    name = property.PropertyType.Name
                },
                can_read = property.CanRead,
                can_write = property.CanWrite
            })
            .ToList();

        var result = template.Render(new
        {
            new_target_type = _newTargetType.Name,
            target_type = _targetType.Name,
            properties = properties
        });

        return result;
    }
}
