using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Sorting;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata
{
    public class PropertyMetadataBuilder<TEntity> : IPropertyMetadataBuilder<TEntity>
    {
        private readonly IDictionary<Type, IDictionary<string, IPropertyMetadata>> _propertyMetadata;
        private readonly IDictionary<Type, IPropertyMetadata> _defaultMetadata;

        protected string displayName;
        protected bool isDefaultSorting;
        protected bool isDefaultSortingDescending;
        protected bool isFilterable;
        protected bool isSortable;
        protected string fullName;
        protected PropertyInfo propertyInfo;

        public PropertyMetadataBuilder(
            IDictionary<Type, IDictionary<string, IPropertyMetadata>> propertyMetadata,
            IDictionary<Type, IPropertyMetadata> defaultMetadata,
            PropertyInfo propertyInfo,
            string fullName)
        {
            if (string.IsNullOrEmpty(fullName))
            {
                throw new ArgumentException($"'{nameof(fullName)}' cannot be null or empty", nameof(fullName));
            }

            _propertyMetadata = propertyMetadata ?? throw new ArgumentNullException(nameof(propertyMetadata));
            _defaultMetadata = defaultMetadata ?? throw new ArgumentNullException(nameof(defaultMetadata));
            this.propertyInfo = propertyInfo ?? throw new ArgumentNullException(nameof(propertyInfo));
            this.fullName = fullName;

            Save(Build());
        }

        public virtual IPropertyMetadata Build()
        {
            return new PropertyMetadata
            {
                DisplayName = displayName,
                IsDefaultSorting = isDefaultSorting,
                IsDefaultSortingDescending = isDefaultSortingDescending,
                IsFilterable = isFilterable,
                IsSortable = isSortable,
                Name = fullName,
                PropertyInfo = propertyInfo,
            };
        }

        public virtual IPropertyMetadataBuilder<TEntity> IsFilterable()
        {
            isFilterable = true;
            Save(Build());

            return this;
        }

        public virtual ISortPropertyMetadataBuilder<TEntity> IsSortable()
        {
            isSortable = true;
            Save(Build());

            return new SortPropertyMetadataBuilder<TEntity>(_propertyMetadata, _defaultMetadata, propertyInfo, fullName, Build());
        }

        public virtual IPropertyMetadataBuilder<TEntity> HasDisplayName(string displayName)
        {
            if (string.IsNullOrWhiteSpace(displayName))
            {
                throw new ArgumentException(
                    $"{nameof(displayName)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(displayName));
            }

            this.displayName = displayName;
            Save(Build());

            return this;
        }

        protected void Save(IPropertyMetadata propertyMetadata)
        {
            if (propertyMetadata == null)
            {
                throw new ArgumentNullException(nameof(propertyMetadata));
            }

            if (propertyMetadata == null)
            {
                throw new ArgumentNullException(nameof(propertyMetadata));
            }

            if (!_propertyMetadata.ContainsKey(typeof(TEntity)))
            {
                _propertyMetadata[typeof(TEntity)] = new Dictionary<string, IPropertyMetadata>();
            }

            if (propertyMetadata.IsDefaultSorting)
            {
                _defaultMetadata[typeof(TEntity)] = propertyMetadata;
            }

            var metadataKey = propertyMetadata.DisplayName ?? propertyMetadata.Name;

            _propertyMetadata[typeof(TEntity)][metadataKey] = propertyMetadata;
        }
    }
}
