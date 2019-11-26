using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Metadata;
using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Sorting
{
    public class SortPropertyMetadataBuilder<TEntity> : PropertyMetadataBuilder<TEntity>, ISortPropertyMetadataBuilder<TEntity>
    {
        public SortPropertyMetadataBuilder(
            IPropertyMetadataMapper strainerPropertyMapper,
            Expression<Func<TEntity, object>> expression,
            IPropertyMetadata basePropertyMetadata)
            : base(strainerPropertyMapper, expression)
        {
            if (strainerPropertyMapper == null)
            {
                throw new ArgumentNullException(nameof(strainerPropertyMapper));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (basePropertyMetadata == null)
            {
                throw new ArgumentNullException(nameof(basePropertyMetadata));
            }

            displayName = basePropertyMetadata.DisplayName;
            isDefaultSorting = basePropertyMetadata.IsDefaultSorting;
            isDefaultSortingDescending = basePropertyMetadata.IsDefaultSortingDescending;
            isFilterable = basePropertyMetadata.IsFilterable;
            isSortable = basePropertyMetadata.IsSortable;
            name = basePropertyMetadata.Name;
            propertyInfo = basePropertyMetadata.PropertyInfo;
        }

        public ISortPropertyMetadataBuilder<TEntity> IsDefaultSort(bool isDescending = false)
        {
            isDefaultSorting = true;
            isDefaultSortingDescending = isDescending;
            UpdateMap(Build());

            return this;
        }
    }
}
