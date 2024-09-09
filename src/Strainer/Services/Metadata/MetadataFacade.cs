using Fluorite.Extensions;
using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata;

public class MetadataFacade : IMetadataFacade
{
    private readonly IEnumerable<IMetadataProvider> _metadataProviders;

    public MetadataFacade(IEnumerable<IMetadataProvider> metadataProviders)
    {
        _metadataProviders = Guard.Against.Null(metadataProviders);
    }

    public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetAllMetadata()
    {
        return GetFirstNotNullResultFromProviders(p => p.GetAllPropertyMetadata())
            ?? new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>().ToReadOnly();
    }

    public IPropertyMetadata GetDefaultMetadata<TEntity>()
    {
        return GetDefaultMetadata(typeof(TEntity));
    }

    public IPropertyMetadata GetDefaultMetadata(Type modelType)
    {
        Guard.Against.Null(modelType);

        return GetFirstNotNullResultFromProviders(p => p.GetDefaultMetadata(modelType));
    }

    public IPropertyMetadata GetMetadata<TEntity>(
        bool isSortableRequired,
        bool isFilterableRequired,
        string name)
    {
        return GetMetadata(typeof(TEntity), isSortableRequired, isFilterableRequired, name);
    }

    public IPropertyMetadata GetMetadata(
        Type modelType,
        bool isSortableRequired,
        bool isFilterableRequired,
        string name)
    {
        Guard.Against.Null(modelType);
        Guard.Against.NullOrWhiteSpace(name);

        return GetFirstNotNullResultFromProviders(
            p => p.GetPropertyMetadata(modelType, isSortableRequired, isFilterableRequired, name));
    }

    public IEnumerable<IPropertyMetadata> GetMetadatas<TEntity>()
    {
        return GetMetadatas(typeof(TEntity));
    }

    public IEnumerable<IPropertyMetadata> GetMetadatas(Type modelType)
    {
        Guard.Against.Null(modelType);

        return GetFirstNotNullResultFromProviders(p => p.GetPropertyMetadatas(modelType));
    }

    private TResult GetFirstNotNullResultFromProviders<TResult>(Func<IMetadataProvider, TResult> resultFunc)
        where TResult : class
    {
        foreach (var provider in _metadataProviders)
        {
            var result = resultFunc.Invoke(provider);
            if (result != null)
            {
                return result;
            }
        }

        return default;
    }
}
