using Fluorite.Extensions;
using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata.Attributes;

public class AttributeMetadataProvider : IMetadataProvider
{
    private readonly IMetadataSourceTypeProvider _metadataSourceTypeProvider;
    private readonly IMetadataAssemblySourceProvider _metadataAssemblySourceProvider;
    private readonly IAttributeMetadataRetriever _attributeMetadataRetriever;

    public AttributeMetadataProvider(
        IMetadataSourceTypeProvider metadataSourceTypeProvider,
        IMetadataAssemblySourceProvider metadataAssemblySourceProvider,
        IAttributeMetadataRetriever attributeMetadataRetriever)
    {
        _metadataSourceTypeProvider = Guard.Against.Null(metadataSourceTypeProvider);
        _metadataAssemblySourceProvider = Guard.Against.Null(metadataAssemblySourceProvider);
        _attributeMetadataRetriever = Guard.Against.Null(attributeMetadataRetriever);
    }

    public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetAllPropertyMetadata()
    {
        var assemblies = _metadataAssemblySourceProvider.GetAssemblies();
        var types = _metadataSourceTypeProvider.GetSourceTypes(assemblies);
        var objectMetadatas = _attributeMetadataRetriever.GetMetadataDictionaryFromObjectAttributes(types);
        var propertyMetadatas = _attributeMetadataRetriever.GetMetadataDictionaryFromPropertyAttributes(types);

        return objectMetadatas
            .MergeLeft(propertyMetadatas)
            .ToReadOnly();
    }

    public IPropertyMetadata GetDefaultMetadata<TEntity>()
    {
        return GetDefaultMetadata(typeof(TEntity));
    }

    public IPropertyMetadata GetDefaultMetadata(Type modelType)
    {
        Guard.Against.Null(modelType);

        var propertyMetadata = _attributeMetadataRetriever.GetDefaultMetadataFromPropertyAttribute(modelType);
        propertyMetadata ??= _attributeMetadataRetriever.GetDefaultMetadataFromObjectAttribute(modelType);

        return propertyMetadata;
    }

    public IPropertyMetadata GetPropertyMetadata<TEntity>(
        bool isSortableRequired,
        bool isFilterableRequired,
        string name)
    {
        Guard.Against.NullOrWhiteSpace(name);

        return GetPropertyMetadata(typeof(TEntity), isSortableRequired, isFilterableRequired, name);
    }

    public IPropertyMetadata GetPropertyMetadata(
        Type modelType,
        bool isSortableRequired,
        bool isFilterableRequired,
        string name)
    {
        Guard.Against.Null(modelType);
        Guard.Against.NullOrWhiteSpace(name);

        var propertyMetadata = _attributeMetadataRetriever.GetMetadataFromPropertyAttribute(modelType, isSortableRequired, isFilterableRequired, name);
        propertyMetadata ??= _attributeMetadataRetriever.GetMetadataFromObjectAttribute(modelType, isSortableRequired, isFilterableRequired, name);

        return propertyMetadata;
    }

    public IReadOnlyList<IPropertyMetadata> GetPropertyMetadatas<TEntity>()
    {
        return GetPropertyMetadatas(typeof(TEntity));
    }

    public IReadOnlyList<IPropertyMetadata> GetPropertyMetadatas(Type modelType)
    {
        Guard.Against.Null(modelType);

        return _attributeMetadataRetriever.GetMetadataFromPropertyAttribute(modelType)
            ?? _attributeMetadataRetriever.GetMetadataFromObjectAttribute(modelType);
    }
}
