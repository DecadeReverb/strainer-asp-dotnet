using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services
{
    public interface IPropertyMetadataProvider
    {
        IPropertyMetadata GetMetadataFromAttributes<TEntity>(
            bool isSortingRequired,
            bool isFilteringRequired,
            string name);
    }
}
