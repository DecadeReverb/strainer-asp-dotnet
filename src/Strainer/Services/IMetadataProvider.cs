using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services
{
    public interface IMetadataProvider
    {
        IPropertyMetadata GetDefaultMetadata<TEntity>();

        IPropertyMetadata GetMetadata<TEntity>(
            bool isSortableRequired,
            bool isFilterableRequired,
            string name);
    }
}
