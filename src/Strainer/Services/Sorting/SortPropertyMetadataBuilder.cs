using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Metadata;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Sorting
{
    public class SortPropertyMetadataBuilder<TEntity> : PropertyMetadataBuilder<TEntity>, ISortPropertyMetadataBuilder<TEntity>
    {
        private readonly IDictionary<Type, IDictionary<string, IPropertyMetadata>> _propertyMetadata;
        private readonly IDictionary<Type, IPropertyMetadata> _defaultMetadata;

        public SortPropertyMetadataBuilder(
            IDictionary<Type, IDictionary<string, IPropertyMetadata>> propertyMetadata,
            IDictionary<Type, IPropertyMetadata> defaultMetadata,
            Expression<Func<TEntity, object>> expression,
            IPropertyMetadata basePropertyMetadata)
            : base(propertyMetadata, defaultMetadata, expression)
        {
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

            _propertyMetadata = propertyMetadata ?? throw new ArgumentNullException(nameof(propertyMetadata));
            _defaultMetadata = defaultMetadata ?? throw new ArgumentNullException(nameof(defaultMetadata));

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
