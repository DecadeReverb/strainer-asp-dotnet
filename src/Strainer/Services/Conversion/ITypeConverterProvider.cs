namespace Fluorite.Strainer.Services.Conversion;

public interface ITypeConverterProvider
{
    ITypeConverter GetTypeConverter(Type type);
}
