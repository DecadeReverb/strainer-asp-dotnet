using System;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Metadata
{
    public class MetadataProvidersWrapper : IMetadataProvidersWrapper
    {
        private readonly IEnumerable<IMetadataProvider> _metadataProviders;

        public MetadataProvidersWrapper(IEnumerable<IMetadataProvider> metadataProviders)
        {
            _metadataProviders = metadataProviders ?? throw new ArgumentNullException(nameof(metadataProviders));
        }

        public IEnumerable<IMetadataProvider> GetMetadataProviders() => _metadataProviders;
    }
}
