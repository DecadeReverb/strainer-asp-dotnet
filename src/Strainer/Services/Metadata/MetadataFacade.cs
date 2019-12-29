using Fluorite.Strainer.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Fluorite.Strainer.Services.Metadata
{
    public class MetadataFacade : IMetadataFacade
    {
        private readonly IMetadataProvidersWrapper _metadataProvidersWrapper;

        public MetadataFacade(IMetadataProvidersWrapper metadataProvidersWrapper)
        {
            _metadataProvidersWrapper = metadataProvidersWrapper ?? throw new ArgumentNullException(nameof(metadataProvidersWrapper));
        }

        public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> GetAllMetadata()
        {
            var metadata = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>();

            foreach (var provider in _metadataProvidersWrapper.GetMetadataProviders())
            {
                var current = provider.GetAllPropertyMetadata();

                if (current != null)
                {
                    return current;
                }
            }

            return new ReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>(metadata);
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

            foreach (var provider in _metadataProvidersWrapper.GetMetadataProviders())
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

            foreach (var provider in _metadataProvidersWrapper.GetMetadataProviders())
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

            foreach (var provider in _metadataProvidersWrapper.GetMetadataProviders())
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
