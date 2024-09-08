using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata;

public interface IMetadataProvider
{
    IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetAllPropertyMetadata();

    IPropertyMetadata GetDefaultMetadata<TEntity>();

    IPropertyMetadata GetDefaultMetadata(Type modelType);

    IPropertyMetadata GetPropertyMetadata<TEntity>(
        bool isSortableRequired,
        bool isFilterableRequired,
        string name);

    IPropertyMetadata GetPropertyMetadata(
        Type modelType,
        bool isSortableRequired,
        bool isFilterableRequired,
        string name);

    IReadOnlyList<IPropertyMetadata> GetPropertyMetadatas<TEntity>();

    IReadOnlyList<IPropertyMetadata> GetPropertyMetadatas(Type modelType);
}
