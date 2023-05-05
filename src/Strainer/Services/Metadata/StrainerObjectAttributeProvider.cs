using Fluorite.Strainer.Attributes;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata
{
    public class StrainerObjectAttributeProvider : IStrainerObjectAttributeProvider
    {
        public StrainerObjectAttribute GetAttribute(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            return type.GetCustomAttribute<StrainerObjectAttribute>(inherit: false);
        }
    }
}
