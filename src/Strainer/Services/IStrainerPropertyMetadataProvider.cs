using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services
{
    public interface IStrainerPropertyMetadataProvider
    {
        IStrainerPropertyMetadata GetMetadataFromAttribute<TEntity>(
            bool isSortingRequired,
            bool isFilteringRequired,
            string name);
    }
}
