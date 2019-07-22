using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services
{
    public interface IPropertyMetadataProvider
    {
        IPropertyMetadata GetMetadataFromAttribute<TEntity>(
            bool isSortingRequired,
            bool isFilteringRequired,
            string name);
    }
}
