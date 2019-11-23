using Fluorite.Strainer.Models;
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
            foreach (var provider in _metadataProviders)
            {
                var metadata = provider.GetDefaultMetadata<TEntity>();

                if (metadata != null)
                {
                    return metadata;
                }
            }

            return null;
        }

        public IPropertyMetadata GetMetadata<TEntity>(bool isSortingRequired, bool isFilteringRequired, string name)
        {
            foreach (var provider in _metadataProviders)
            {
                var metadata = provider.GetMetadata<TEntity>(isSortingRequired, isFilteringRequired, name);

                if (metadata != null)
                {
                    return metadata;
                }
            }

            return null;
        }
    }
}
