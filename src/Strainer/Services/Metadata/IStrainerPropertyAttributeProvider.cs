using Fluorite.Strainer.Attributes;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IStrainerPropertyAttributeProvider
    {
        StrainerPropertyAttribute GetAttribute(PropertyInfo propertyInfo);
    }
}
