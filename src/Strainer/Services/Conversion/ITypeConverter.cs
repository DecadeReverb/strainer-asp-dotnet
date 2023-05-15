namespace Fluorite.Strainer.Services.Conversion
{
    public interface ITypeConverter
    {
        bool CanConvertFrom(Type type);

        object ConvertFrom(object value);
    }
}
