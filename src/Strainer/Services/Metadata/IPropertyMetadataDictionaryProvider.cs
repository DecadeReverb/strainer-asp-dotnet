using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IPropertyMetadataDictionaryProvider
    {
        IReadOnlyDictionary<string, IPropertyMetadata> GetMetadata(Type type);

        IReadOnlyDictionary<string, IPropertyMetadata> GetMetadata(Type type, StrainerObjectAttribute strainerObjectAttribute);
    }
}