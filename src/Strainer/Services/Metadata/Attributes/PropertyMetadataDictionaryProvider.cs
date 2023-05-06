using Fluorite.Extensions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata.Attributes
{
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
            _propertyInfoProvider = propertyInfoProvider ?? throw new ArgumentNullException(nameof(propertyInfoProvider));
            _strainerAttributeProvider = strainerAttributeProvider ?? throw new ArgumentNullException(nameof(strainerAttributeProvider));
            _attributePropertyMetadataBuilder = attributePropertyMetadataBuilder ?? throw new ArgumentNullException(nameof(attributePropertyMetadataBuilder));
        }

        public IReadOnlyDictionary<string, IPropertyMetadata> GetMetadata(Type type)
        {
            return _propertyInfoProvider.GetPropertyInfos(type)
                .Select(propertyInfo => _strainerAttributeProvider.GetPropertyAttribute(propertyInfo))
                .Where(attribute => attribute != null)
                .ToDictionary(attribute => attribute.Name, attribute => (IPropertyMetadata)attribute)
                .ToReadOnly();
        }

        public IReadOnlyDictionary<string, IPropertyMetadata> GetMetadata(Type type, StrainerObjectAttribute strainerObjectAttribute)
        {
            return _propertyInfoProvider.GetPropertyInfos(type)
                .Select(propertyInfo => _attributePropertyMetadataBuilder.BuildPropertyMetadata(strainerObjectAttribute, propertyInfo))
                .ToDictionary(metadata => metadata.Name, metadata => metadata)
                .ToReadOnly();
        }
    }
}
