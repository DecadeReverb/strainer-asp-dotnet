using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata;

public class MetadataSourceChecker : IMetadataSourceChecker
{
    private readonly IStrainerOptionsProvider _strainerOptionsProvider;

    public MetadataSourceChecker(IStrainerOptionsProvider strainerOptionsProvider)
    {
        _strainerOptionsProvider = Guard.Against.Null(strainerOptionsProvider);
    }

    public bool IsMetadataSourceEnabled(MetadataSourceType metadataSourceType)
    {
        return _strainerOptionsProvider
            .GetStrainerOptions()
            .MetadataSourceType
            .HasFlag(metadataSourceType);
    }
}
