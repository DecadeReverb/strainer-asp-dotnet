using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata;

public interface IMetadataFacade
{
    IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetAllMetadata();

    IPropertyMetadata? GetDefaultMetadata<TEntity>();

    IPropertyMetadata? GetDefaultMetadata(Type modelType);

    IPropertyMetadata? GetMetadata<TEntity>(
        bool isSortableRequired,
        bool isFilterableRequired,
        string name);

    IPropertyMetadata? GetMetadata(
        Type modelType,
        bool isSortableRequired,
        bool isFilterableRequired,
        string name);

    IEnumerable<IPropertyMetadata> GetMetadatas<TEntity>();

    IEnumerable<IPropertyMetadata> GetMetadatas(Type modelType);
}
