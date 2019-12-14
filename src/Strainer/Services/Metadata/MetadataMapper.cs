using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Metadata
{
    public class MetadataMapper : IMetadataMapper, IPropertyMetadataProvider
    {
        private readonly Dictionary<Type, IPropertyMetadata> _defaultPropertyMetadata;
        private readonly Dictionary<Type, IDictionary<string, IPropertyMetadata>> _propertyMetadata;
        private readonly Dictionary<Type, IObjectMetadata> _objectMetadata;
        private readonly StrainerOptions _options;

        public MetadataMapper(IStrainerOptionsProvider optionsProvider)
        {
            _defaultPropertyMetadata = new Dictionary<Type, IPropertyMetadata>();
            _propertyMetadata = new Dictionary<Type, IDictionary<string, IPropertyMetadata>>();
            _objectMetadata = new Dictionary<Type, IObjectMetadata>();
            _options = (optionsProvider ?? throw new ArgumentNullException(nameof(optionsProvider)))
                .GetStrainerOptions();
        }

        public void AddObjectMetadata<TEntity>(IObjectMetadata objectMetadata)
        {
            if (objectMetadata == null)
            {
                throw new ArgumentNullException(nameof(objectMetadata));
            }

            if (!_options.MetadataSourceType.HasFlag(MetadataSourceType.FluentApi))
            {
                throw new InvalidOperationException(
                    $"Current {nameof(MetadataSourceType)} setting does not " +
                    $"allow support {nameof(MetadataSourceType.FluentApi)}. " +
                    $"Include {nameof(MetadataSourceType.FluentApi)} option to " +
                    $"be able to use it.");
            }

            _objectMetadata[typeof(TEntity)] = objectMetadata;
        }

        public void AddPropertyMetadata<TEntity>(IPropertyMetadata propertyMetadata)
        {
            if (propertyMetadata == null)
            {
                throw new ArgumentNullException(nameof(propertyMetadata));
            }

            if (!_options.MetadataSourceType.HasFlag(MetadataSourceType.FluentApi))
            {
                throw new InvalidOperationException(
                    $"Current {nameof(MetadataSourceType)} setting does not " +
                    $"allow support {nameof(MetadataSourceType.FluentApi)}. " +
                    $"Include {nameof(MetadataSourceType.FluentApi)} option to " +
                    $"be able to use it.");
            }

            if (!_propertyMetadata.ContainsKey(typeof(TEntity)))
            {
                _propertyMetadata[typeof(TEntity)] = new Dictionary<string, IPropertyMetadata>();
            }

            if (propertyMetadata.IsDefaultSorting)
            {
                _defaultPropertyMetadata[typeof(TEntity)] = propertyMetadata;
            }

            var metadataKey = propertyMetadata.DisplayName ?? propertyMetadata.Name;

            _propertyMetadata[typeof(TEntity)][metadataKey] = propertyMetadata;
        }

        public IPropertyMetadata GetDefaultMetadata<TEntity>()
        {
            return GetDefaultMetadata(typeof(TEntity));
        }

        public IPropertyMetadata GetDefaultMetadata(Type modelType)
        {
            if (modelType is null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            if (!IsMetadataSourceEnabled(MetadataSourceType.FluentApi))
            {
                return null;
            }

            _defaultPropertyMetadata.TryGetValue(modelType, out var propertyMetadata);

            if (propertyMetadata == null)
            {
                if (_objectMetadata.TryGetValue(modelType, out var objectMetadata))
                {
                    propertyMetadata = new PropertyMetadata
                    {
                        IsDefaultSorting = true,
                        IsDefaultSortingDescending = objectMetadata.IsDefaultSortingDescending,
                        IsFilterable = objectMetadata.IsFilterable,
                        IsSortable = objectMetadata.IsSortable,
                        Name = objectMetadata.DefaultSortingPropertyName,
                        PropertyInfo = objectMetadata.DefaultSortingPropertyInfo,
                    };
                }
            }

            return propertyMetadata;
        }

        public IPropertyMetadata GetPropertyMetadata<TEntity>(
            bool isSortableRequired,
            bool isFilterableRequired,
            string name)
        {
            return GetPropertyMetadata(typeof(TEntity), isSortableRequired, isFilterableRequired, name);
        }

        public IPropertyMetadata GetPropertyMetadata(
            Type modelType,
            bool isSortableRequired,
            bool isFilterableRequired,
            string name)
        {
            if (modelType is null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            if (!IsMetadataSourceEnabled(MetadataSourceType.FluentApi))
            {
                return null;
            }

            if (_propertyMetadata.TryGetValue(modelType, out IDictionary<string, IPropertyMetadata> metadataSet))
            {
                var comparisonMethod = _options.IsCaseInsensitiveForNames
                    ? StringComparison.OrdinalIgnoreCase
                    : StringComparison.Ordinal;

                var propertyMetadata = metadataSet.FirstOrDefault(pair =>
                {
                    return pair.Key.Equals(name, comparisonMethod)
                        && (!isSortableRequired || pair.Value.IsSortable)
                        && (!isFilterableRequired || pair.Value.IsFilterable);
                }).Value;

                if (propertyMetadata != null)
                {
                    return propertyMetadata;
                }
            }

            if (_objectMetadata.TryGetValue(modelType, out var objectMetadata))
            {
                if ((!isSortableRequired || objectMetadata.IsSortable)
                    && (!isFilterableRequired || objectMetadata.IsFilterable))
                {
                    return new PropertyMetadata
                    {
                        IsDefaultSorting = true,
                        IsDefaultSortingDescending = objectMetadata.IsDefaultSortingDescending,
                        IsFilterable = objectMetadata.IsFilterable,
                        IsSortable = objectMetadata.IsSortable,
                        Name = objectMetadata.DefaultSortingPropertyName,
                        PropertyInfo = objectMetadata.DefaultSortingPropertyInfo,
                    };
                }
            }

            return null;
        }

        public IEnumerable<IPropertyMetadata> GetPropertyMetadatas<TEntity>()
        {
            return GetPropertyMetadatas(typeof(TEntity));
        }

        public IEnumerable<IPropertyMetadata> GetPropertyMetadatas(Type modelType)
        {
            if (modelType is null)
            {
                throw new ArgumentNullException(nameof(modelType));
            }

            if (_propertyMetadata.TryGetValue(modelType, out var metadatas))
            {
                return metadatas.Values;
            }

            return null;
        }

        public IObjectMetadataBuilder<TEntity> Object<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (!_options.MetadataSourceType.HasFlag(MetadataSourceType.FluentApi))
            {
                throw new InvalidOperationException(
                    $"Current {nameof(MetadataSourceType)} setting does not " +
                    $"allow support {nameof(MetadataSourceType.FluentApi)}. " +
                    $"Include {nameof(MetadataSourceType.FluentApi)} option to " +
                    $"be able to use it.");
            }

            return new ObjectMetadataBuilder<TEntity>(this, expression);
        }

        public IPropertyMetadataBuilder<TEntity> Property<TEntity>(Expression<Func<TEntity, object>> expression)
        {
            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            if (!_options.MetadataSourceType.HasFlag(MetadataSourceType.FluentApi))
            {
                throw new InvalidOperationException(
                    $"Current {nameof(MetadataSourceType)} setting does not " +
                    $"allow support {nameof(MetadataSourceType.FluentApi)}. " +
                    $"Include {nameof(MetadataSourceType.FluentApi)} option to " +
                    $"be able to use it.");
            }

            if (!_propertyMetadata.ContainsKey(typeof(TEntity)))
            {
                _propertyMetadata[typeof(TEntity)] = new Dictionary<string, IPropertyMetadata>();
            }

            return new PropertyMetadataBuilder<TEntity>(this, expression);
        }

        private bool IsMetadataSourceEnabled(MetadataSourceType metadataSourceType)
            => _options.MetadataSourceType.HasFlag(metadataSourceType);
    }
}
