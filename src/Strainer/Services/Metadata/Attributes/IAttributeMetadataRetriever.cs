using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata.Attributes
{
    public interface IAttributeMetadataRetriever
    {
        IPropertyMetadata GetDefaultMetadataFromObjectAttribute(Type modelType);

        IPropertyMetadata GetDefaultMetadataFromPropertyAttribute(Type modelType);

        IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetMetadataDictionaryFromObjectAttributes(ICollection<Type> types);

        IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetMetadataDictionaryFromPropertyAttributes(ICollection<Type> types);

        IPropertyMetadata GetMetadataFromObjectAttribute(Type modelType, bool isSortableRequired, bool isFilterableRequired, string name);

        IPropertyMetadata GetMetadataFromPropertyAttribute(Type modelType, bool isSortableRequired, bool isFilterableRequired, string name);

        IEnumerable<IPropertyMetadata> GetMetadataFromObjectAttribute(Type modelType);

        IEnumerable<IPropertyMetadata> GetMetadataFromPropertyAttribute(Type modelType);
    }
}