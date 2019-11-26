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
        private readonly Dictionary<Type, ISet<IPropertyMetadata>> _propertyMetadata;
        private readonly Dictionary<Type, IObjectMetadata> _objectMetadata;
        private readonly StrainerOptions _options;

        public MetadataMapper(IStrainerOptionsProvider optionsProvider)
        {
            _defaultPropertyMetadata = new Dictionary<Type, IPropertyMetadata>();
            _propertyMetadata = new Dictionary<Type, ISet<IPropertyMetadata>>();
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
                _propertyMetadata[typeof(TEntity)] = new HashSet<IPropertyMetadata>();
            }

            _defaultPropertyMetadata[typeof(TEntity)] = propertyMetadata;
            _propertyMetadata[typeof(TEntity)].Add(propertyMetadata);
        }

        public IPropertyMetadata GetDefaultMetadata<TEntity>()
        {
            if (!IsMetadataSourceEnabled(MetadataSourceType.FluentApi))
            {
                return null;
            }

            _defaultPropertyMetadata.TryGetValue(typeof(TEntity), out var propertyMetadata);

            if (propertyMetadata == null)
            {
                if (_objectMetadata.TryGetValue(typeof(TEntity), out var objectMetadata))
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

        public IPropertyMetadata GetMetadata<TEntity>(
            bool isSortableRequired,
            bool isFilterableRequired,
            string name)
        {
            if (!IsMetadataSourceEnabled(MetadataSourceType.FluentApi))
            {
                return null;
            }

            if (_propertyMetadata.TryGetValue(typeof(TEntity), out ISet<IPropertyMetadata> metadataSet))
            {
                var comparisonMethod = _options.IsCaseInsensitiveForNames
                    ? StringComparison.OrdinalIgnoreCase
                    : StringComparison.Ordinal;

                var propertyMetadata = metadataSet.FirstOrDefault(metadata =>
                {
                    return (metadata.DisplayName ?? metadata.Name).Equals(name, comparisonMethod)
                        && (!isSortableRequired || metadata.IsSortable)
                        && (!isFilterableRequired || metadata.IsFilterable);
                });

                if (propertyMetadata != null)
                {
                    return propertyMetadata;
                }
            }

            if (_objectMetadata.TryGetValue(typeof(TEntity), out var objectMetadata))
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
                _propertyMetadata[typeof(TEntity)] = new HashSet<IPropertyMetadata>();
            }

            return new PropertyMetadataBuilder<TEntity>(this, expression);
        }

        private bool IsMetadataSourceEnabled(MetadataSourceType metadataSourceType)
            => _options.MetadataSourceType.HasFlag(metadataSourceType);
    }
}
