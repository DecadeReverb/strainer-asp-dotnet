using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IMainMetadataProvider
    {
        IPropertyMetadata GetDefaultMetadata<TEntity>();

        IPropertyMetadata GetMetadata<TEntity>(
            bool isSortingRequired,
            bool isFilteringRequired,
            string name);
    }
}
