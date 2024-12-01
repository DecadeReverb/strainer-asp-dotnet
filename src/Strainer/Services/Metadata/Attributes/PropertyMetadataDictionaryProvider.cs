using Fluorite.Extensions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata.Attributes;

public class PropertyMetadataDictionaryProvider : IPropertyMetadataDictionaryProvider
{
    private readonly IPropertyInfoProvider _propertyInfoProvider;
    private readonly IStrainerAttributeProvider _strainerAttributeProvider;
    private readonly IAttributePropertyMetadataBuilder _attributePropertyMetadataBuilder;

    public PropertyMetadataDictionaryProvider(
        IPropertyInfoProvider propertyInfoProvider,
        IStrainerAttributeProvider strainerAttributeProvider,
        IAttributePropertyMetadataBuilder attributePropertyMetadataBuilder)
    {
        _propertyInfoProvider = Guard.Against.Null(propertyInfoProvider);
        _strainerAttributeProvider = Guard.Against.Null(strainerAttributeProvider);
        _attributePropertyMetadataBuilder = Guard.Against.Null(attributePropertyMetadataBuilder);
    }

    public IReadOnlyDictionary<string, IPropertyMetadata> GetMetadata(Type type)
    {
        Guard.Against.Null(type);

        return _propertyInfoProvider.GetPropertyInfos(type)
            .Select(propertyInfo => _strainerAttributeProvider.GetPropertyAttribute(propertyInfo))
            .Where(attribute => attribute is not null)
            .Select(attribute => attribute!)
            .ToDictionary(attribute => attribute.Name, attribute => (IPropertyMetadata)attribute)
            .ToReadOnly();
    }

    public IReadOnlyDictionary<string, IPropertyMetadata> GetMetadata(Type type, StrainerObjectAttribute strainerObjectAttribute)
    {
        Guard.Against.Null(type);
        Guard.Against.Null(strainerObjectAttribute);

        return _propertyInfoProvider.GetPropertyInfos(type)
            .Select(propertyInfo => _attributePropertyMetadataBuilder.BuildPropertyMetadata(strainerObjectAttribute, propertyInfo))
            .ToDictionary(metadata => metadata.Name, metadata => metadata)
            .ToReadOnly();
    }
}
