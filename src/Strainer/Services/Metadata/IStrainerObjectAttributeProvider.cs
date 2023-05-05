using Fluorite.Strainer.Attributes;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IStrainerObjectAttributeProvider
    {
        StrainerObjectAttribute GetAttribute(Type type);
    }
}
