using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services
{
    public interface IStrainerPropertyMetadataProvider
    {
        IStrainerPropertyMetadata GetMetadata<TEntity>(
            bool isSortingRequired,
            bool ifFileringRequired,
            string name,
            bool includeAttributes = true);

        IStrainerPropertyMetadata GetMetadataFromAttribute<TEntity>(bool isSortingRequired, bool isFilteringRequired, string name);
    }
}
