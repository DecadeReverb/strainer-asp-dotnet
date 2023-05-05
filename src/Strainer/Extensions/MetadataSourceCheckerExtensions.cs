using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Metadata;

namespace Fluorite.Strainer.Extensions
{
    public static class MetadataSourceCheckerExtensions
    {
        public static bool IsFluentApiEnabled(this IMetadataSourceChecker metadataSourceChecker)
        {
            if (metadataSourceChecker is null)
            {
                throw new ArgumentNullException(nameof(metadataSourceChecker));
            }

            return metadataSourceChecker.IsMetadataSourceEnabled(MetadataSourceType.FluentApi);
        }

        public static bool IsAttributeSourceEnabled(this IMetadataSourceChecker metadataSourceChecker)
        {
            if (metadataSourceChecker is null)
            {
                throw new ArgumentNullException(nameof(metadataSourceChecker));
            }

            return metadataSourceChecker.IsMetadataSourceEnabled(MetadataSourceType.Attributes);
        }
    }
}
