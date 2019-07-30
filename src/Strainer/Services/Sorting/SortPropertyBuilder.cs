using Fluorite.Strainer.Models;
using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Sorting
{
    public class SortPropertyBuilder<TEntity> : PropertyBuilder<TEntity>, ISortPropertyBuilder<TEntity>, IPropertyBuilder<TEntity>
    {
        public SortPropertyBuilder(
            IPropertyMapper strainerPropertyMapper,
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

            DisplayNameProperty = basePropertyMetadata.DisplayName;
            IsDefaultSortingProperty = basePropertyMetadata.IsDefaultSorting;
            IsDefaultSortingDescendingProperty = basePropertyMetadata.IsDefaultSortingDescending;
            IsFilterableProperty = basePropertyMetadata.IsFilterable;
            IsSortableProperty = basePropertyMetadata.IsSortable;
            NameProperty = basePropertyMetadata.Name;
            PropertyInfoProperty = basePropertyMetadata.PropertyInfo;
        }

        public ISortPropertyBuilder<TEntity> IsDefaultSort(bool isDescending = false)
        {
            IsDefaultSortingProperty = true;
            IsDefaultSortingDescendingProperty = isDescending;
            UpdateMap(Build());

            return this;
        }
    }
}
