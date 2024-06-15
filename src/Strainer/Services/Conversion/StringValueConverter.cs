using Fluorite.Strainer.Exceptions;

namespace Fluorite.Strainer.Services.Conversion;

public class StringValueConverter : IStringValueConverter
{
    public object Convert(string value, Type targetType, ITypeConverter typeConverter)
    {
        if (value is null)
        {
            throw new ArgumentNullException(nameof(value));
        }

        if (targetType is null)
        {
            throw new ArgumentNullException(nameof(targetType));
        }

        if (typeConverter is null)
        {
            throw new ArgumentNullException(nameof(typeConverter));
        }

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
