using Fluorite.Strainer.Exceptions;
using System.Reflection;

namespace Fluorite.Strainer.Services.Conversion
{
    public class StringValueConverter : IStringValueConverter
    {
        public object Convert(string value, PropertyInfo propertyInfo, ITypeConverter typeConverter)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (propertyInfo is null)
            {
                throw new ArgumentNullException(nameof(propertyInfo));
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
                    $"to type '{propertyInfo.PropertyType.FullName}'.",
                    ex,
                    value,
                    propertyInfo.PropertyType);
            }
        }
    }
}
