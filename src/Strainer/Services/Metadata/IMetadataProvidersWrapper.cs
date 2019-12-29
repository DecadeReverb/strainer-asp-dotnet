using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IMetadataProvidersWrapper
    {
        IEnumerable<IMetadataProvider> GetMetadataProviders();
    }
}
