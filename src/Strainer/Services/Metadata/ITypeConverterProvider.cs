namespace Fluorite.Strainer.Services.Metadata
{
    public interface ITypeConverterProvider
    {
        ITypeConverter GetTypeConverter(Type type);
    }
}
