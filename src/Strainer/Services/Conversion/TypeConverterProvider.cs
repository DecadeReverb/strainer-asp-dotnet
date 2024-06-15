using System.ComponentModel;

namespace Fluorite.Strainer.Services.Conversion;

public class TypeConverterProvider : ITypeConverterProvider
{
    public ITypeConverter GetTypeConverter(Type type)
    {
        Guard.Against.Null(type);

        var typeConverter = TypeDescriptor.GetConverter(type);

        return new ComponentModelTypeConverter(typeConverter);
    }
}
