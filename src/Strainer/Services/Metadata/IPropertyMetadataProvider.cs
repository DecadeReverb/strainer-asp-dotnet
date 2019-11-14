using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IPropertyMetadataProvider
    {
        IPropertyMetadata GetDefaultMetadata<TEntity>();

        IPropertyMetadata GetMetadata<TEntity>(bool isSortableRequired, bool isFilterableRequired, string name);
    }
}
