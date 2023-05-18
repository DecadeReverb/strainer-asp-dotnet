using Fluorite.Strainer.Attributes;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata.Attributes
{
    public class StrainerAttributeProvider : IStrainerAttributeProvider
    {
        public StrainerObjectAttribute GetObjectAttribute(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.GetCustomAttribute<StrainerObjectAttribute>(inherit: false);
        }

        public StrainerPropertyAttribute GetPropertyAttribute(PropertyInfo propertyInfo)
        {
            if (propertyInfo is null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
            }

            var attribute = propertyInfo.GetCustomAttribute<StrainerPropertyAttribute>(inherit: false);
            if (attribute != null && attribute.PropertyInfo == null)
            {
                attribute.PropertyInfo = propertyInfo;
            }

            return attribute;
        }
    }
}
