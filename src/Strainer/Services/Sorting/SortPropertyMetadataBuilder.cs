using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Metadata;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fluorite.Strainer.Services.Sorting
{
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

            displayName = basePropertyMetadata.DisplayName;
            isDefaultSorting = basePropertyMetadata.IsDefaultSorting;
            isDefaultSortingDescending = basePropertyMetadata.IsDefaultSortingDescending;
            isFilterable = basePropertyMetadata.IsFilterable;
            isSortable = basePropertyMetadata.IsSortable;
            base.fullName = basePropertyMetadata.Name;

            Save(Build());
        }

        public ISortPropertyMetadataBuilder<TEntity> IsDefaultSort(bool isDescending = false)
        {
            isDefaultSorting = true;
            isDefaultSortingDescending = isDescending;
            Save(Build());

            return this;
        }
    }
}
