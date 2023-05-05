using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IAttributeMetadataRetriever
    {
        IPropertyMetadata GetDefaultMetadataFromPropertyAttribute(Type modelType);

        IPropertyMetadata GetMetadataFromObjectAttribute(Type modelType, bool isSortableRequired, bool isFilterableRequired, string name);

        IPropertyMetadata GetMetadataFromPropertyAttribute(Type modelType, bool isSortableRequired, bool isFilterableRequired, string name);

        IEnumerable<IPropertyMetadata> GetMetadatasFromObjectAttribute(Type modelType);

        IEnumerable<IPropertyMetadata> GetMetadatasFromPropertyAttribute(Type modelType);
    }
}