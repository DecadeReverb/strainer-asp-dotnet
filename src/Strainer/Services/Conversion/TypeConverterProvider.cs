using System.ComponentModel;
using Fluorite.Strainer.Services.Metadata;

namespace Fluorite.Strainer.Services.Conversion
{
    public class TypeConverterProvider : ITypeConverterProvider
    {
        public ITypeConverter GetTypeConverter(Type type)
        {
            var typeConverter = TypeDescriptor.GetConverter(type);

            return new ComponentModelTypeConverter(typeConverter);
        }
    }
}
