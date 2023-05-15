using System.ComponentModel;
using Fluorite.Strainer.Services.Metadata;

namespace Fluorite.Strainer.Services.Conversion
{
    public class TypeConverterProvider : ITypeConverterProvider
    {
        public ITypeConverter GetTypeConverter(Type type)
        {
            if (type is null)
            {
                throw new ArgumentNullException(nameof(type));
            }

            var typeConverter = TypeDescriptor.GetConverter(type);

            return new ComponentModelTypeConverter(typeConverter);
        }
    }
}
