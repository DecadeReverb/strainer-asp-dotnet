using Fluorite.Strainer.Exceptions;

namespace Fluorite.Strainer.Services.Conversion;

public class StringValueConverter : IStringValueConverter
{
    public object Convert(string value, Type targetType, ITypeConverter typeConverter)
    {
        Guard.Against.Null(value);
        Guard.Against.Null(targetType);
        Guard.Against.Null(typeConverter);

        try
        {
            return typeConverter.ConvertFrom(value);
        }
        catch (Exception ex)
        {
            throw new StrainerConversionException(
                $"Failed to convert value '{value}' " +
                $"to type '{targetType.FullName}'.",
                ex,
                value,
                targetType);
        }
    }
}
