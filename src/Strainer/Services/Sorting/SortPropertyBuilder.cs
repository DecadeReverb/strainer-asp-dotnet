using Fluorite.Strainer.Models;
using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Sorting
{
    public class SortPropertyBuilder<TEntity> : PropertyBuilder<TEntity>, ISortPropertyBuilder<TEntity>, IPropertyBuilder<TEntity>
    {
        private readonly PropertyMetadata _propertyMetadata;

        public SortPropertyBuilder(
            IPropertyMapper strainerPropertyMapper,
            Expression<Func<TEntity, object>> expression,
            IPropertyMetadata basePropertyMetadata)
            : base(strainerPropertyMapper, expression)
        {
            _propertyMetadata = new PropertyMetadata
            {
                DisplayName = basePropertyMetadata.DisplayName,
                IsDefaultSorting = basePropertyMetadata.IsDefaultSorting,
                IsDefaultSortingDescending = basePropertyMetadata.IsDefaultSortingDescending,
                IsFilterable = basePropertyMetadata.IsFilterable,
                IsSortable = basePropertyMetadata.IsSortable,
                Name = basePropertyMetadata.Name,
                 PropertyInfo = basePropertyMetadata.PropertyInfo,
            };
        }

        public ISortPropertyBuilder<TEntity> IsDefaultSort(bool isDescending = false)
        {
            _propertyMetadata.IsDefaultSorting = true;
            _propertyMetadata.IsDefaultSortingDescending = isDescending;
            UpdateMap(_propertyMetadata);

            return this;
        }
    }
}
