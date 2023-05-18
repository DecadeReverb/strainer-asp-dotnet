using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata.Attributes
{
    public interface IPropertyMetadataDictionaryProvider
    {
        IReadOnlyDictionary<string, IPropertyMetadata> GetMetadata(Type type);

        IReadOnlyDictionary<string, IPropertyMetadata> GetMetadata(Type type, StrainerObjectAttribute strainerObjectAttribute);
    }
}