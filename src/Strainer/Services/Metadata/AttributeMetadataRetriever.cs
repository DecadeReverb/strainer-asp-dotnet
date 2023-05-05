using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata
{
    public class AttributeMetadataRetriever : IAttributeMetadataRetriever
    {
        private readonly IMetadataSourceChecker _metadataSourceChecker;
        private readonly IAttributePropertyMetadataBuilder _attributePropertyMetadataBuilder;
        private readonly IStrainerObjectAttributeProvider _strainerObjectAttributeProvider;
        private readonly IStrainerPropertyAttributeProvider _strainerPropertyAttributeProvider;
        private readonly IPropertyInfoProvider _propertyInfoProvider;

        public AttributeMetadataRetriever(
            IMetadataSourceChecker metadataSourceChecker,
            IAttributePropertyMetadataBuilder attributePropertyMetadataBuilder,
            IStrainerObjectAttributeProvider strainerObjectAttributeProvider,
            IStrainerPropertyAttributeProvider strainerPropertyAttributeProvider,
            IPropertyInfoProvider propertyInfoProvider)
        {
            _metadataSourceChecker = metadataSourceChecker ?? throw new ArgumentNullException(nameof(metadataSourceChecker));
            _attributePropertyMetadataBuilder = attributePropertyMetadataBuilder ?? throw new ArgumentNullException(nameof(attributePropertyMetadataBuilder));
            _strainerObjectAttributeProvider = strainerObjectAttributeProvider ?? throw new ArgumentNullException(nameof(strainerObjectAttributeProvider));
            _strainerPropertyAttributeProvider = strainerPropertyAttributeProvider ?? throw new ArgumentNullException(nameof(strainerPropertyAttributeProvider));
            _propertyInfoProvider = propertyInfoProvider ?? throw new ArgumentNullException(nameof(propertyInfoProvider));
        }

        public IPropertyMetadata GetDefaultMetadataFromPropertyAttribute(Type modelType)
        {
            if (!IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes))
            {
                return null;
            }

            var attribute = modelType
                .GetProperties()
                .Select(propertyInfo => _strainerPropertyAttributeProvider.GetAttribute(propertyInfo))
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

        public IPropertyMetadata GetMetadataFromObjectAttribute(
            Type modelType,
            bool isSortableRequired,
            bool isFilterableRequired,
            string name)
        {
            if (!IsMetadataSourceEnabled(MetadataSourceType.ObjectAttributes))
            {
                return null;
            }

            var currentType = modelType;

            do
            {
                var attribute = _strainerObjectAttributeProvider.GetAttribute(currentType);
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

        public IEnumerable<IPropertyMetadata> GetMetadatasFromObjectAttribute(Type modelType)
        {
            if (!IsMetadataSourceEnabled(MetadataSourceType.ObjectAttributes))
            {
                return null;
            }

            var currentType = modelType;

            do
            {
                var attribute = _strainerObjectAttributeProvider.GetAttribute(currentType);
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

        public IPropertyMetadata GetMetadataFromPropertyAttribute(
            Type modelType,
            bool isSortableRequired,
            bool isFilterableRequired,
            string name)
        {
            if (!IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes))
            {
                return null;
            }

            var keyValue = modelType
                .GetProperties()
                .Select(propertyInfo =>
                {
                    var attribute = _strainerPropertyAttributeProvider.GetAttribute(propertyInfo);

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

        public IEnumerable<IPropertyMetadata> GetMetadatasFromPropertyAttribute(Type modelType)
        {
            if (!IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes))
            {
                return null;
            }

            var metadata = modelType
                .GetProperties()
                .Select(propertyInfo => _strainerPropertyAttributeProvider.GetAttribute(propertyInfo))
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
}
