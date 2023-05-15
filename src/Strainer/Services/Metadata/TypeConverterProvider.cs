using System.ComponentModel;

namespace Fluorite.Strainer.Services.Metadata
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
