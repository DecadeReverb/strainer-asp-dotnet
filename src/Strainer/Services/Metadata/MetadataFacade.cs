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
        var defaultValue = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>().ToReadOnly();

        return GetFirstNotNullResultFromProviders(p => p.GetAllPropertyMetadata(), defaultValue);
    }

    public IPropertyMetadata GetDefaultMetadata<TEntity>()
    {
        return GetDefaultMetadata(typeof(TEntity));
    }

    public IPropertyMetadata GetDefaultMetadata(Type modelType)
    {
        Guard.Against.Null(modelType);

        return GetFirstNotNullResultFromProviders(p => p.GetDefaultMetadata(modelType), defaultValue: null);
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
            p => p.GetPropertyMetadata(modelType, isSortableRequired, isFilterableRequired, name),
            defaultValue: null);
    }

    public IEnumerable<IPropertyMetadata> GetMetadatas<TEntity>()
    {
        return GetMetadatas(typeof(TEntity));
    }

    public IEnumerable<IPropertyMetadata> GetMetadatas(Type modelType)
    {
        Guard.Against.Null(modelType);

        return GetFirstNotNullResultFromProviders(p => p.GetPropertyMetadatas(modelType), defaultValue: null);
    }

    private TResult GetFirstNotNullResultFromProviders<TResult>(Func<IMetadataProvider, TResult> resultFunc, TResult defaultValue)
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

        return defaultValue;
    }
}
