using Fluorite.Strainer.Attributes;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata
{
    public class StrainerPropertyAttributeProvider : IStrainerPropertyAttributeProvider
    {
        public StrainerPropertyAttribute GetAttribute(PropertyInfo propertyInfo)
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
