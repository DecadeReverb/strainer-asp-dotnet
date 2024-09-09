using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata;

public interface IMetadataProvider
{
    IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetAllPropertyMetadata();

    IPropertyMetadata GetDefaultMetadata(Type modelType);

    IPropertyMetadata GetPropertyMetadata(
        Type modelType,
        bool isSortableRequired,
        bool isFilterableRequired,
        string name);

    IReadOnlyList<IPropertyMetadata> GetPropertyMetadatas(Type modelType);
}
