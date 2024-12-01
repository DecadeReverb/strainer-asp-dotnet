using Fluorite.Strainer.Attributes;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata.Attributes;

public class StrainerAttributeProvider : IStrainerAttributeProvider
{
    public StrainerObjectAttribute? GetObjectAttribute(Type type)
    {
        Guard.Against.Null(type);

        return type.GetCustomAttribute<StrainerObjectAttribute>(inherit: false);
    }

    public StrainerPropertyAttribute? GetPropertyAttribute(PropertyInfo propertyInfo)
    {
        Guard.Against.Null(propertyInfo);

        var attribute = propertyInfo.GetCustomAttribute<StrainerPropertyAttribute>(inherit: false);
        if (attribute != null && attribute.PropertyInfo == null)
        {
            attribute.PropertyInfo = propertyInfo;
        }

        return attribute;
    }
}
