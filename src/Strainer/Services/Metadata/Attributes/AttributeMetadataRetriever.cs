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

    public AttributeMetadataRetriever(
        IMetadataSourceChecker metadataSourceChecker,
        IAttributePropertyMetadataBuilder attributePropertyMetadataBuilder,
        IPropertyMetadataDictionaryProvider propertyMetadataDictionaryProvider,
        IStrainerAttributeProvider strainerAttributeProvider,
        IPropertyInfoProvider propertyInfoProvider)
    {
        _metadataSourceChecker = metadataSourceChecker ?? throw new ArgumentNullException(nameof(metadataSourceChecker));
        _attributePropertyMetadataBuilder = attributePropertyMetadataBuilder ?? throw new ArgumentNullException(nameof(attributePropertyMetadataBuilder));
        _strainerAttributeProvider = strainerAttributeProvider ?? throw new ArgumentNullException(nameof(strainerAttributeProvider));
        _propertyInfoProvider = propertyInfoProvider ?? throw new ArgumentNullException(nameof(propertyInfoProvider));
        _propertyMetadataDictionaryProvider = propertyMetadataDictionaryProvider ?? throw new ArgumentNullException(nameof(propertyMetadataDictionaryProvider));
    }

    public IPropertyMetadata GetDefaultMetadataFromObjectAttribute(Type modelType)
    {
        if (modelType is null)
        {
            throw new ArgumentNullException(nameof(modelType));
        }

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
        if (!IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes))
        {
            return null;
        }

        var attribute = _propertyInfoProvider
            .GetPropertyInfos(modelType)
            .Select(propertyInfo => _strainerAttributeProvider.GetPropertyAttribute(propertyInfo))
            .Where(attribute => attribute != null)
            .FirstOrDefault(attribute => attribute.IsDefaultSorting);

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

    public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetMetadataDictionaryFromObjectAttributes(ICollection<Type> types)
    {
        if (types is null)
        {
            throw new ArgumentNullException(nameof(types));
        }

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

    public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetMetadataDictionaryFromPropertyAttributes(ICollection<Type> types)
    {
        if (types is null)
        {
            throw new ArgumentNullException(nameof(types));
        }

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
        if (modelType is null)
        {
            throw new ArgumentNullException(nameof(modelType));
        }

        if (!IsMetadataSourceEnabled(MetadataSourceType.ObjectAttributes))
        {
            return null;
        }

        var currentType = modelType;

        do
        {
            var attribute = _strainerAttributeProvider.GetObjectAttribute(currentType);
            var propertyInfo = _propertyInfoProvider.GetPropertyInfo(currentType, name);

            if (attribute != null
                && propertyInfo != null
                && (!isSortableRequired || attribute.IsSortable)
                && (!isFilterableRequired || attribute.IsFilterable))
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
        if (modelType is null)
        {
            throw new ArgumentNullException(nameof(modelType));
        }

        if (!IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes))
        {
            return null;
        }

        var keyValue = _propertyInfoProvider
            .GetPropertyInfos(modelType)
            .Select(propertyInfo =>
            {
                var attribute = _strainerAttributeProvider.GetPropertyAttribute(propertyInfo);

                return new
                {
                    propertyInfo,
                    attribute,
                };
            })
            .Where(x => x.attribute != null)
            .FirstOrDefault(x =>
            {
                var propertyInfo = x.propertyInfo;
                var attribute = x.attribute;

                return (!isSortableRequired || attribute.IsSortable)
                    && (!isFilterableRequired || attribute.IsFilterable)
                    && (attribute.DisplayName ?? attribute.Name ?? propertyInfo.Name).Equals(name);
            });

        return keyValue?.attribute;
    }

    public IEnumerable<IPropertyMetadata> GetMetadataFromObjectAttribute(Type modelType)
    {
        if (modelType is null)
        {
            throw new ArgumentNullException(nameof(modelType));
        }

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
                    .Select(propertyInfo => _attributePropertyMetadataBuilder.BuildPropertyMetadata(attribute, propertyInfo));
            }

            currentType = currentType.BaseType;
        }
        while (currentType != typeof(object) && currentType != typeof(ValueType));

        return null;
    }

    public IEnumerable<IPropertyMetadata> GetMetadataFromPropertyAttribute(Type modelType)
    {
        if (modelType is null)
        {
            throw new ArgumentNullException(nameof(modelType));
        }

        if (!IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes))
        {
            return null;
        }

        var metadata = _propertyInfoProvider
            .GetPropertyInfos(modelType)
            .Select(propertyInfo => _strainerAttributeProvider.GetPropertyAttribute(propertyInfo))
            .Where(attribute => attribute != null)
            .Select(attribute => (IPropertyMetadata)attribute);

        return metadata.Any()
            ? metadata
            : null;
    }

    private bool IsMetadataSourceEnabled(MetadataSourceType metadataSourceType)
    {
        return _metadataSourceChecker.IsMetadataSourceEnabled(metadataSourceType);
    }
}
