using Fluorite.Strainer.Models.Metadata;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IPropertyMetadataProvider
    {
        IPropertyMetadata GetDefaultMetadata<TEntity>();

        IPropertyMetadata GetMetadata<TEntity>(bool isSortableRequired, bool isFilterableRequired, string name);
    }
}
