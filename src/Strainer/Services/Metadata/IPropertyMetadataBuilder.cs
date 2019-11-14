using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.Services.Metadata
{
    public interface IPropertyMetadataBuilder<TEntity>
    {
        IPropertyMetadata Build();
        IPropertyMetadataBuilder<TEntity> IsFilterable();
        ISortPropertyMetadataBuilder<TEntity> IsSortable();
        IPropertyMetadataBuilder<TEntity> HasDisplayName(string displayName);
    }
}
