using System.Reflection;

namespace Fluorite.Strainer.Services.Conversion
{
    public interface IStringValueConverter
    {
        object Convert(string value, PropertyInfo propertyInfo, ITypeConverter typeConverter);
    }
}