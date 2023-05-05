using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IMetadataSourceChecker
    {
        bool IsMetadataSourceEnabled(MetadataSourceType metadataSourceType);
    }
}
