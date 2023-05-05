using Fluorite.Extensions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata
{
    public class PropertyMetadataDictionaryProvider : IPropertyMetadataDictionaryProvider
    {
        private readonly IPropertyInfoProvider _propertyInfoProvider;
        private readonly IStrainerPropertyAttributeProvider _strainerPropertyAttributeProvider;
        private readonly IAttributePropertyMetadataBuilder _attributePropertyMetadataBuilder;

        public PropertyMetadataDictionaryProvider(
            IPropertyInfoProvider propertyInfoProvider,
            IStrainerPropertyAttributeProvider strainerPropertyAttributeProvider,
            IAttributePropertyMetadataBuilder attributePropertyMetadataBuilder)
        {
            _propertyInfoProvider = propertyInfoProvider ?? throw new ArgumentNullException(nameof(propertyInfoProvider));
            _strainerPropertyAttributeProvider = strainerPropertyAttributeProvider ?? throw new ArgumentNullException(nameof(strainerPropertyAttributeProvider));
            _attributePropertyMetadataBuilder = attributePropertyMetadataBuilder ?? throw new ArgumentNullException(nameof(attributePropertyMetadataBuilder));
        }

        public IReadOnlyDictionary<string, IPropertyMetadata> GetMetadata(Type type)
        {
            return _propertyInfoProvider.GetPropertyInfos(type)
                .Select(propertyInfo => _strainerPropertyAttributeProvider.GetAttribute(propertyInfo))
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
