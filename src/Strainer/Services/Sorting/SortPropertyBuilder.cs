using Fluorite.Strainer.Models;
using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Sorting
{
    public class SortPropertyBuilder<TEntity> : StrainerPropertyBuilder<TEntity>, ISortPropertyBuilder<TEntity>, IStrainerPropertyBuilder<TEntity>
    {
        private readonly StrainerPropertyMetadata _propertyMetadata;

        public SortPropertyBuilder(
            IStrainerPropertyMapper strainerPropertyMapper,
            Expression<Func<TEntity, object>> expression,
            IStrainerPropertyMetadata basePropertyMetadata)
            : base(strainerPropertyMapper, expression)
        {
            _propertyMetadata = new StrainerPropertyMetadata
            {
                DisplayName = basePropertyMetadata.DisplayName,
                IsDefaultSortOrder = basePropertyMetadata.IsDefaultSortOrder,
                IsDefaultSortOrderAscending = basePropertyMetadata.IsDefaultSortOrderAscending,
                IsFilterable = basePropertyMetadata.IsFilterable,
                IsSortable = basePropertyMetadata.IsSortable,
                Name = basePropertyMetadata.Name,
                 PropertyInfo = basePropertyMetadata.PropertyInfo,
            };
        }

        public ISortPropertyBuilder<TEntity> IsDefaultSort(bool isAscending = true)
        {
            _propertyMetadata.IsDefaultSortOrder = true;
            _propertyMetadata.IsDefaultSortOrderAscending = isAscending;
            UpdateMap(_propertyMetadata);

            return this;
        }
    }
}
