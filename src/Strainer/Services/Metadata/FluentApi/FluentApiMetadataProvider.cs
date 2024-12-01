using Fluorite.Extensions;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Configuration;

namespace Fluorite.Strainer.Services.Metadata.FluentApi;

public class FluentApiMetadataProvider : IMetadataProvider
{
    private readonly IStrainerOptionsProvider _strainerOptionsProvider;
    private readonly IConfigurationMetadataProvider _metadataProvider;
    private readonly IFluentApiPropertyMetadataBuilder _propertyMetadataBuilder;
    private readonly IPropertyInfoProvider _propertyInfoProvider;

    public FluentApiMetadataProvider(
        IStrainerOptionsProvider strainerOptionsProvider,
        IConfigurationMetadataProvider metadataProvider,
        IPropertyInfoProvider propertyInfoProvider,
        IFluentApiPropertyMetadataBuilder propertyMetadataBuilder)
    {
        _strainerOptionsProvider = Guard.Against.Null(strainerOptionsProvider);
        _metadataProvider = Guard.Against.Null(metadataProvider);
        _propertyInfoProvider = Guard.Against.Null(propertyInfoProvider);
        _propertyMetadataBuilder = Guard.Against.Null(propertyMetadataBuilder);
    }

    public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>? GetAllPropertyMetadata()
    {
        if (!IsFluentApiEnabled())
        {
            return null;
        }

        var objectMetadata = _metadataProvider.GetObjectMetadata();

        return _metadataProvider
            .GetPropertyMetadata()
            .Keys
            .Union(objectMetadata.Keys)
            .Select(type => (type, BuildMetadataKeyValuePair(type)))
            .ToDictionary(tuple => tuple.type, tuple => tuple.Item2)
            .ToReadOnly();
    }

    public IPropertyMetadata? GetDefaultMetadata(Type modelType)
    {
        Guard.Against.Null(modelType);

        if (!IsFluentApiEnabled())
        {
            return null;
        }

        _metadataProvider.GetDefaultMetadata().TryGetValue(modelType, out var propertyMetadata);

        if (propertyMetadata == null)
        {
            if (_metadataProvider.GetObjectMetadata().TryGetValue(modelType, out var objectMetadata))
            {
                propertyMetadata = _propertyMetadataBuilder.BuildPropertyMetadata(objectMetadata);
            }
        }

        return propertyMetadata;
    }

    public IPropertyMetadata? GetPropertyMetadata(
        Type modelType,
        bool isSortableRequired,
        bool isFilterableRequired,
        string name)
    {
        Guard.Against.Null(modelType);
        Guard.Against.NullOrWhiteSpace(name);

        if (!IsFluentApiEnabled())
        {
            return null;
        }

        if (_metadataProvider.GetPropertyMetadata().TryGetValue(modelType, out var propertyMetadatas))
        {
            var propertyMetadata = propertyMetadatas
                .Where(pair =>
                {
                    return pair.Key.Equals(name)
                        && (!isSortableRequired || pair.Value.IsSortable)
                        && (!isFilterableRequired || pair.Value.IsFilterable);
                })
                .FirstOrDefault().Value;

            return propertyMetadata;
        }

        if (_metadataProvider.GetObjectMetadata().TryGetValue(modelType, out var objectMetadata))
        {
            if (((isFilterableRequired && objectMetadata.IsFilterable) || (!isFilterableRequired))
                && ((isSortableRequired && objectMetadata.IsSortable) || (!isSortableRequired)))
            {
                var propertyInfo = _propertyInfoProvider.GetPropertyInfo(modelType, name);
                if (propertyInfo is not null)
                {
                    return _propertyMetadataBuilder.BuildPropertyMetadataFromPropertyInfo(objectMetadata, propertyInfo);
                }
            }
        }

        return null;
    }

    public IReadOnlyList<IPropertyMetadata>? GetPropertyMetadatas(Type modelType)
    {
        Guard.Against.Null(modelType);

        if (!IsFluentApiEnabled())
        {
            return null;
        }

        if (_metadataProvider.GetPropertyMetadata().TryGetValue(modelType, out var metadatas))
        {
            return metadatas.Values.ToList().AsReadOnly();
        }

        if (_metadataProvider.GetObjectMetadata().TryGetValue(modelType, out var objectMetadata))
        {
            return _propertyInfoProvider.GetPropertyInfos(modelType)
                .Select(p => _propertyMetadataBuilder.BuildPropertyMetadataFromPropertyInfo(objectMetadata, p))
                .ToList();
        }

        return null;
    }

    private IReadOnlyDictionary<string, IPropertyMetadata> BuildMetadataKeyValuePair(Type type)
    {
        // TODO: Shouldn't property metadata override object metadata, but still be returned?
        // So type-wide config is set with object call, but property call overrides that for some special case?
        var propertyMetadataDictionary = _metadataProvider.GetPropertyMetadata();
        if (propertyMetadataDictionary.TryGetValue(type, out var metadatas))
        {
            return metadatas;
        }

        var objectMetadata = _metadataProvider.GetObjectMetadata()[type];

        return GetPropertyMetadatasFromObjectMetadata(type, objectMetadata);
    }

    private IReadOnlyDictionary<string, IPropertyMetadata> GetPropertyMetadatasFromObjectMetadata(Type type, IObjectMetadata objectMetadata)
    {
        return _propertyInfoProvider
            .GetPropertyInfos(type)
            .Select(propertyInfo => _propertyMetadataBuilder.BuildPropertyMetadataFromPropertyInfo(objectMetadata, propertyInfo))
            .ToDictionary(p => p.Name, p => p)
            .ToReadOnlyDictionary();
    }

    private bool IsFluentApiEnabled() => IsMetadataSourceEnabled(MetadataSourceType.FluentApi);

    private bool IsMetadataSourceEnabled(MetadataSourceType metadataSourceType)
    {
        return _strainerOptionsProvider
            .GetStrainerOptions()
            .MetadataSourceType
            .HasFlag(metadataSourceType);
    }
}
