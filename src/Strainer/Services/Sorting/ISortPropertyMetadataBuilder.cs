using Fluorite.Strainer.Services.Metadata;

namespace Fluorite.Strainer.Services.Sorting;

public interface ISortPropertyMetadataBuilder<TEntity> : IPropertyMetadataBuilder<TEntity>
{
    ISortPropertyMetadataBuilder<TEntity> IsDefaultSort(bool isDescending = false);
}
