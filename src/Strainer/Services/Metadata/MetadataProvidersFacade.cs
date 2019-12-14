using Fluorite.Strainer.Models.Metadata;
using System;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Metadata
{
    public class MetadataProvidersFacade : IMetadataProvidersFacade
    {
        private readonly IEnumerable<IPropertyMetadataProvider> _metadataProviders;

        public MetadataProvidersFacade(IEnumerable<IPropertyMetadataProvider> metadataProviders)
        {
            if (metadataProviders is null)
            {
                throw new ArgumentNullException(nameof(metadataProviders));
            }

            _metadataProviders = metadataProviders;
        }

        public IPropertyMetadata GetDefaultMetadata<TEntity>()
        {
            return GetDefaultMetadata(typeof(TEntity));
        }

        public IPropertyMetadata GetDefaultMetadata(Type modelType)
        {
            if (modelType is null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            foreach (var provider in _metadataProviders)
            {
                var metadata = provider.GetDefaultMetadata(modelType);

                if (metadata != null)
                {
                    return metadata;
                }
            }

            return null;
        }

        public IPropertyMetadata GetMetadata<TEntity>(
            bool isSortableRequired,
            bool isFilterableRequired,
            string name)
        {
            return GetMetadata(typeof(TEntity), isSortableRequired, isFilterableRequired, name);
        }

        public IPropertyMetadata GetMetadata(
            Type modelType,
            bool isSortableRequired,
            bool isFilterableRequired,
            string name)
        {
            if (modelType is null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            foreach (var provider in _metadataProviders)
            {
                var metadata = provider.GetPropertyMetadata(modelType, isSortableRequired, isFilterableRequired, name);

                if (metadata != null)
                {
                    return metadata;
                }
            }

            return null;
        }

        public IEnumerable<IPropertyMetadata> GetMetadatas<TEntity>()
        {
            return GetMetadatas(typeof(TEntity));
        }

        public IEnumerable<IPropertyMetadata> GetMetadatas(Type modelType)
        {
            if (modelType is null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            foreach (var provider in _metadataProviders)
            {
                var metadatas = provider.GetPropertyMetadatas(modelType);

                if (metadatas != null)
                {
                    return metadatas;
                }
            }

            return null;
        }
    }
}
