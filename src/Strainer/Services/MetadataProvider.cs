using Fluorite.Strainer.Models;
using System;

namespace Fluorite.Strainer.Services
{
    public class MetadataProvider : IMetadataProvider
    {
        private readonly IPropertyMapper _propertyMapper;
        private readonly IAttributeMetadataProvider _attributeMetadataProvider;
        private readonly StrainerOptions _options;

        public MetadataProvider(
            IPropertyMapper propertyMapper,
            IAttributeMetadataProvider attributeMetadataProvider,
            IStrainerOptionsProvider strainerOptionsProvider)
        {
            _propertyMapper = propertyMapper ?? throw new ArgumentNullException(nameof(propertyMapper));
            _attributeMetadataProvider = attributeMetadataProvider ?? throw new ArgumentNullException(nameof(attributeMetadataProvider));
            _options = (strainerOptionsProvider ?? throw new ArgumentNullException(nameof(strainerOptionsProvider)))
                .GetStrainerOptions();
        }

        public IPropertyMetadata GetDefaultMetadata<TEntity>()
        {
            if (IsMetadataSourceEnabled(MetadataSourceType.None))
            {
                throw new InvalidOperationException(
                    $"No metadata can be retrieved when its source type is set " +
                    $"to {MetadataSourceType.None}. Change metadata source type " +
                    $"to different settings to use this method.");
            }

            IPropertyMetadata metadata = null;

            if (IsMetadataSourceEnabled(MetadataSourceType.FluentApi))
            {
                metadata = _propertyMapper.GetDefaultMetadata<TEntity>();
            }

            if (IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes) && metadata == null)
            {
                metadata = _attributeMetadataProvider.GetDefaultMetadataFromPropertyAttribute<TEntity>();
            }

            if (IsMetadataSourceEnabled(MetadataSourceType.ObjectAttributes) && metadata == null)
            {
                metadata = _attributeMetadataProvider.GetDefaultMetadataFromObjectAttribute<TEntity>();
            }

            return metadata;
        }

        public IPropertyMetadata GetMetadata<TEntity>(
            bool isSortableRequired,
            bool isFilterableRequired,
            string name)
        {
            if (IsMetadataSourceEnabled(MetadataSourceType.None))
            {
                throw new InvalidOperationException(
                    $"No metadata can be retrieved when its source type is set " +
                    $"to {MetadataSourceType.None}. Change metadata source type " +
                    $"to different settings to use this method.");
            }

            IPropertyMetadata metadata = null;

            if (IsMetadataSourceEnabled(MetadataSourceType.FluentApi))
            {
                metadata = _propertyMapper.GetMetadata<TEntity>(isFilterableRequired, isSortableRequired, name);
            }

            if (IsMetadataSourceEnabled(MetadataSourceType.PropertyAttributes) && metadata == null)
            {
                metadata = _attributeMetadataProvider.GetMetadataFromPropertyAttribute<TEntity>(
                    isSortableRequired,
                    isFilterableRequired,
                    name);
            }

            if (IsMetadataSourceEnabled(MetadataSourceType.ObjectAttributes) && metadata == null)
            {
                metadata = _attributeMetadataProvider.GetMetadataFromObjectAttribute<TEntity>(
                    isSortableRequired,
                    isFilterableRequired,
                    name);
            }

            return metadata;
        }

        private bool IsMetadataSourceEnabled(MetadataSourceType metadataSourceType)
            => _options.MetadataSourceType.HasFlag(metadataSourceType);
    }
}
