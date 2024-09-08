using Fluorite.Extensions;
using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata.Attributes;

public class AttributeMetadataRetriever : IAttributeMetadataRetriever
{
    private readonly IMetadataSourceChecker _metadataSourceChecker;
    private readonly IAttributePropertyMetadataBuilder _attributePropertyMetadataBuilder;
    private readonly IPropertyMetadataDictionaryProvider _propertyMetadataDictionaryProvider;
    private readonly IStrainerAttributeProvider _strainerAttributeProvider;
    private readonly IPropertyInfoProvider _propertyInfoProvider;
    private readonly IAttributeCriteriaChecker _attributeCriteriaChecker;

    public AttributeMetadataRetriever(
        IMetadataSourceChecker metadataSourceChecker,
        IAttributePropertyMetadataBuilder attributePropertyMetadataBuilder,
        IPropertyMetadataDictionaryProvider propertyMetadataDictionaryProvider,
        IStrainerAttributeProvider strainerAttributeProvider,
        IPropertyInfoProvider propertyInfoProvider,
        IAttributeCriteriaChecker attributeCriteriaChecker)
    {
        _metadataSourceChecker = Guard.Against.Null(metadataSourceChecker);
        _attributePropertyMetadataBuilder = Guard.Against.Null(attributePropertyMetadataBuilder);
        _strainerAttributeProvider = Guard.Against.Null(strainerAttributeProvider);
        _propertyInfoProvider = Guard.Against.Null(propertyInfoProvider);
        _propertyMetadataDictionaryProvider = Guard.Against.Null(propertyMetadataDictionaryProvider);
        _attributeCriteriaChecker = Guard.Against.Null(attributeCriteriaChecker);
    }

    public IPropertyMetadata GetDefaultMetadataFromObjectAttribute(Type modelType)
    {
        Guard.Against.Null(modelType);

        if (!IsMetadataSourceEnabled(MetadataSourceType.ObjectAttributes))
        {
            return null;
        }

        var currentType = modelType;

        do
        {
            var attribute = _strainerAttributeProvider.GetObjectAttribute(currentType);

            if (attribute != null && attribute.DefaultSortingPropertyName != null)
            {
                var propertyInfo = _propertyInfoProvider.GetPropertyInfo(modelType, attribute.DefaultSortingPropertyName);
                if (propertyInfo == null)
                {
                    throw new InvalidOperationException(
                        $"Could not find property {attribute.DefaultSortingPropertyName} " +
                        $"in type {modelType.FullName} marked as its default " +
                        $"sorting property. Ensure that such property exists in " +
                        $"{modelType.FullName} and it's accessible.");
                }

                return _attributePropertyMetadataBuilder.BuildDefaultPropertyMetadata(attribute, propertyInfo);
            }

            currentType = currentType.BaseType;

        }
        while (currentType != typeof(object) && currentType != typeof(ValueType));

        return null;
    }

    public IPropertyMetadata GetDefaultMetadataFromPropertyAttribute(Type modelType)
    {
        Guard.Against.Null(modelType);

        if (!IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes))
        {
            return null;
        }

        var attribute = _propertyInfoProvider
            .GetPropertyInfos(modelType)
            .Select(propertyInfo => _strainerAttributeProvider.GetPropertyAttribute(propertyInfo))
            .FirstOrDefault(attribute => attribute is not null && attribute.IsDefaultSorting);

        if (attribute != null)
        {
            if (!attribute.IsSortable)
            {
                throw new InvalidOperationException(
                    $"Property {attribute.PropertyInfo.Name} on {attribute.PropertyInfo.DeclaringType.FullName} " +
                    $"is declared as {nameof(IPropertyMetadata.IsDefaultSorting)} " +
                    $"but not as {nameof(IPropertyMetadata.IsSortable)}. " +
                    $"Set the {nameof(IPropertyMetadata.IsSortable)} to true " +
                    $"in order to use the property as a default sortable property.");
            }
        }

        return attribute;
    }

    public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetMetadataDictionaryFromObjectAttributes(
        ICollection<Type> types)
    {
        Guard.Against.Null(types);

        // TODO: Shouldn't this return empty dictionary if attribute-based metadata is disabled?
        return types
            .Select(type => new
            {
                Type = type,
                Attribute = _strainerAttributeProvider.GetObjectAttribute(type),
            })
            .Where(x => x.Attribute != null)
            .Select(x => new
            {
                x.Type,
                Metadatas = _propertyMetadataDictionaryProvider.GetMetadata(x.Type, x.Attribute),
            })
            .ToDictionary(x => x.Type, x => x.Metadatas)
            .ToReadOnly();
    }

    public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetMetadataDictionaryFromPropertyAttributes(
        ICollection<Type> types)
    {
        Guard.Against.Null(types);

        return types
            .Select(type => new
            {
                Type = type,
                Attributes = _propertyMetadataDictionaryProvider.GetMetadata(type),
            })
            .Where(x => x.Attributes.Any())
            .ToDictionary(x => x.Type, x => x.Attributes)
            .ToReadOnly();
    }

    public IPropertyMetadata GetMetadataFromObjectAttribute(
        Type modelType,
        bool isSortableRequired,
        bool isFilterableRequired,
        string name)
    {
        Guard.Against.Null(modelType);
        Guard.Against.NullOrWhiteSpace(name);

        if (!IsMetadataSourceEnabled(MetadataSourceType.ObjectAttributes))
        {
            return null;
        }

        var currentType = modelType;

        do
        {
            var attribute = _strainerAttributeProvider.GetObjectAttribute(currentType);
            var propertyInfo = _propertyInfoProvider.GetPropertyInfo(currentType, name);
            var isMatching = _attributeCriteriaChecker.CheckIfObjectAttributeIsMatching(
                attribute,
                propertyInfo,
                isSortableRequired,
                isFilterableRequired);

            if (isMatching)
            {
                return _attributePropertyMetadataBuilder.BuildPropertyMetadata(attribute, propertyInfo);
            }

            currentType = currentType.BaseType;

        }
        while (currentType != typeof(object) && currentType != typeof(ValueType));

        return null;
    }

    public IPropertyMetadata GetMetadataFromPropertyAttribute(
        Type modelType,
        bool isSortableRequired,
        bool isFilterableRequired,
        string name)
    {
        Guard.Against.Null(modelType);
        Guard.Against.NullOrWhiteSpace(name);

        if (!IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes))
        {
            return null;
        }

        var keyValue = _propertyInfoProvider
            .GetPropertyInfos(modelType)
            .Select(propertyInfo => new
            {
                propertyInfo,
                attribute = _strainerAttributeProvider.GetPropertyAttribute(propertyInfo),
            })
            .FirstOrDefault(x =>
            {
                var propertyInfo = x.propertyInfo;
                var attribute = x.attribute;

                return _attributeCriteriaChecker.CheckIfPropertyAttributeIsMatching(
                    attribute,
                    propertyInfo,
                    isSortableRequired,
                    isFilterableRequired,
                    name);
            });

        return keyValue?.attribute;
    }

    public IReadOnlyList<IPropertyMetadata> GetMetadataFromObjectAttribute(Type modelType)
    {
        Guard.Against.Null(modelType);

        if (!IsMetadataSourceEnabled(MetadataSourceType.ObjectAttributes))
        {
            return null;
        }

        var currentType = modelType;

        do
        {
            var attribute = _strainerAttributeProvider.GetObjectAttribute(currentType);
            if (attribute != null)
            {
                return _propertyInfoProvider.GetPropertyInfos(currentType)
                    .Select(propertyInfo => _attributePropertyMetadataBuilder.BuildPropertyMetadata(attribute, propertyInfo))
                    .ToList()
                    .AsReadOnly();
            }

            currentType = currentType.BaseType;
        }
        while (currentType != typeof(object) && currentType != typeof(ValueType));

        return null;
    }

    public IReadOnlyList<IPropertyMetadata> GetMetadataFromPropertyAttribute(Type modelType)
    {
        Guard.Against.Null(modelType);

        if (!IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes))
        {
            return null;
        }

        var metadata = _propertyInfoProvider
            .GetPropertyInfos(modelType)
            .Select(propertyInfo => _strainerAttributeProvider.GetPropertyAttribute(propertyInfo))
            .Where(attribute => attribute != null)
            .Cast<IPropertyMetadata>()
            .ToList()
            .AsReadOnly();

        return metadata.Any()
            ? metadata
            : null;
    }

    private bool IsMetadataSourceEnabled(MetadataSourceType metadataSourceType)
    {
        return _metadataSourceChecker.IsMetadataSourceEnabled(metadataSourceType);
    }
}
