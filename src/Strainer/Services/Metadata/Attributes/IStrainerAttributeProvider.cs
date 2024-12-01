using Fluorite.Strainer.Attributes;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata.Attributes;

public interface IStrainerAttributeProvider
{
    StrainerPropertyAttribute? GetPropertyAttribute(PropertyInfo propertyInfo);

    StrainerObjectAttribute? GetObjectAttribute(Type type);
}
