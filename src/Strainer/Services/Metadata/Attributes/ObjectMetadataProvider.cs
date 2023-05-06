using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models.Metadata;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata.Attributes
{
    public class ObjectMetadataProvider : IObjectMetadataProvider
    {
        private readonly IStrainerOptionsProvider _strainerOptionsProvider;
        private readonly IAttributePropertyMetadataBuilder _attributePropertyMetadataBuilder;

        public ObjectMetadataProvider(
            IStrainerOptionsProvider strainerOptionsProvider,
            IAttributePropertyMetadataBuilder attributePropertyMetadataBuilder)
        {
            _strainerOptionsProvider = strainerOptionsProvider
                ?? throw new ArgumentNullException(nameof(strainerOptionsProvider));
            _attributePropertyMetadataBuilder = attributePropertyMetadataBuilder
                ?? throw new ArgumentNullException(nameof(attributePropertyMetadataBuilder));
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
                // TODO: Use Stariner object attribute provider.
                var attribute = currentType.GetCustomAttribute<StrainerObjectAttribute>(inherit: false);

                if (attribute != null && attribute.DefaultSortingPropertyName != null)
                {
                    // TODO: User Property provider.
                    var propertyInfo = modelType.GetProperty(
                        attribute.DefaultSortingPropertyName,
                        BindingFlags.Public | BindingFlags.Instance);

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

        private bool IsMetadataSourceEnabled(MetadataSourceType metadataSourceType)
        {
            // TODO: User metadata source type checker.
            return _strainerOptionsProvider
                .GetStrainerOptions()
                .MetadataSourceType
                .HasFlag(metadataSourceType);
        }
    }
}
