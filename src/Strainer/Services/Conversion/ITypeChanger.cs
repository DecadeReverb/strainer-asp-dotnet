namespace Fluorite.Strainer.Services.Conversion
{
    public interface ITypeChanger
    {
        object ChangeType(string value, Type targetType);
    }
}