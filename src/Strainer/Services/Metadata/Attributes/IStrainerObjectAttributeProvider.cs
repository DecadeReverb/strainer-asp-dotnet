using Fluorite.Strainer.Attributes;

namespace Fluorite.Strainer.Services.Metadata.Attributes
{
    public interface IStrainerObjectAttributeProvider
    {
        StrainerObjectAttribute GetAttribute(Type type);
    }
}
