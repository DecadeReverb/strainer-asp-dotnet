using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Metadata;
using System.Reflection;

namespace Fluorite.Strainer.Services.Sorting;

public class SortPropertyMetadataBuilder<TEntity> : PropertyMetadataBuilder<TEntity>, ISortPropertyMetadataBuilder<TEntity>
{
    public SortPropertyMetadataBuilder(
        IDictionary<Type, IDictionary<string, IPropertyMetadata>> propertyMetadata,
        IDictionary<Type, IPropertyMetadata> defaultMetadata,
        PropertyInfo propertyInfo,
        string fullName,
        IPropertyMetadata basePropertyMetadata)
        : base(propertyMetadata, defaultMetadata, propertyInfo, fullName)
    {
        if (basePropertyMetadata is null)
        {
            throw new ArgumentNullException(nameof(basePropertyMetadata));
        }

        DisplayName = basePropertyMetadata.DisplayName;
        IsDefaultSorting = basePropertyMetadata.IsDefaultSorting;
        IsDefaultSortingDescending = basePropertyMetadata.IsDefaultSortingDescending;
        IsFilterableValue = basePropertyMetadata.IsFilterable;
        IsSortableValue = basePropertyMetadata.IsSortable;

        Save(Build());
    }

    public ISortPropertyMetadataBuilder<TEntity> IsDefaultSort(bool isDescending = false)
    {
        IsDefaultSorting = true;
        IsDefaultSortingDescending = isDescending;
        Save(Build());

        return this;
    }
}
