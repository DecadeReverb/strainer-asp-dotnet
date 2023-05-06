using Fluorite.Strainer.Attributes;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata.Attributes
{
    public interface IStrainerPropertyAttributeProvider
    {
        StrainerPropertyAttribute GetAttribute(PropertyInfo propertyInfo);
    }
}
