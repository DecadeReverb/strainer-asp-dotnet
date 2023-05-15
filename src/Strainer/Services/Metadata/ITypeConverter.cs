namespace Fluorite.Strainer.Services.Metadata
{
    public interface ITypeConverter
    {
        bool CanConvertFrom(Type type);

        object ConvertFrom(object value);
    }
}
