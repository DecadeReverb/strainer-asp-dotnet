using Fluorite.Strainer.Exceptions;

namespace Fluorite.Strainer.Services.Conversion
{
    public class TypeChanger : ITypeChanger
    {
        public object ChangeType(string value, Type targetType)
        {
            if (value is null)
            {
                throw new ArgumentNullException(nameof(value));
            }

            if (targetType is null)
            {
                throw new ArgumentNullException(nameof(targetType));
            }

            try
            {
                return Convert.ChangeType(value, targetType);
            }
            catch (Exception ex)
            {
                throw new StrainerConversionException(
                    $"Failed to change type of filter value '{value}' " +
                    $"to type '{targetType.FullName}'.",
                    ex,
                    value,
                    targetType);
            }
        }
    }
}
