using Fluorite.Strainer.Models.Metadata;
using System.Collections.ObjectModel;

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
        var metadata = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>();

        foreach (var provider in _metadataProviders)
        {
            var current = provider.GetAllPropertyMetadata();

            if (current != null)
            {
                return current;
            }
        }

        return new ReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>(metadata);
    }

    public IPropertyMetadata GetDefaultMetadata<TEntity>()
    {
        return GetDefaultMetadata(typeof(TEntity));
    }

    public IPropertyMetadata GetDefaultMetadata(Type modelType)
    {
        Guard.Against.Null(modelType);

        foreach (var provider in _metadataProviders)
        {
            var metadata = provider.GetDefaultMetadata(modelType);

            if (metadata != null)
            {
                return metadata;
            }
        }

        return null;
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

        foreach (var provider in _metadataProviders)
        {
            var metadata = provider.GetPropertyMetadata(modelType, isSortableRequired, isFilterableRequired, name);

            if (metadata != null)
            {
                return metadata;
            }
        }

        return null;
    }

    public IEnumerable<IPropertyMetadata> GetMetadatas<TEntity>()
    {
        return GetMetadatas(typeof(TEntity));
    }

    public IEnumerable<IPropertyMetadata> GetMetadatas(Type modelType)
    {
        Guard.Against.Null(modelType);

        foreach (var provider in _metadataProviders)
        {
            var metadatas = provider.GetPropertyMetadatas(modelType);

            if (metadatas != null)
            {
                return metadatas;
            }
        }

        return null;
    }
}
