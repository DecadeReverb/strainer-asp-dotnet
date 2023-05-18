namespace Fluorite.Strainer.Services.Conversion
{
    public interface IStringValueConverter
    {
        object Convert(string value, Type targetType, ITypeConverter typeConverter);
    }
}