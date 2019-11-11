using Fluorite.Strainer.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services
{
    public class PropertyMapper : IPropertyMapper
    {
        private readonly Dictionary<Type, IPropertyMetadata> _defaultPropertyMetadata;
        private readonly Dictionary<Type, ISet<IPropertyMetadata>> _propertyMetadata;
        private readonly StrainerOptions _options;

        public PropertyMapper(IStrainerOptionsProvider optionsProvider)
        {
            _defaultPropertyMetadata = new Dictionary<Type, IPropertyMetadata>();
            _propertyMetadata = new Dictionary<Type, ISet<IPropertyMetadata>>();
            _options = (optionsProvider ?? throw new ArgumentNullException(nameof(optionsProvider)))
                .GetStrainerOptions();
        }

        public void AddMetadata<TEntity>(IPropertyMetadata propertyMetadata)
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

        public IReadOnlyDictionary<Type, IEnumerable<IPropertyMetadata>> GetAllMetadata()
        {
            var newdict = _propertyMetadata.ToDictionary(k => k.Key, v => v.Value as IEnumerable<IPropertyMetadata>);

            return new ReadOnlyDictionary<Type, IEnumerable<IPropertyMetadata>>(newdict);
        }

        public IEnumerable<IPropertyMetadata> GetAllMetadata<TEntity>()
        {
            _propertyMetadata.TryGetValue(typeof(TEntity), out var propertyMetadatas);

            return propertyMetadatas?.ToList().AsReadOnly();
        }

        public IPropertyMetadata GetDefaultMetadata<TEntity>()
        {
            _defaultPropertyMetadata.TryGetValue(typeof(TEntity), out var propertyMetadata);

            return propertyMetadata;
        }

        public IPropertyMetadata GetMetadata<TEntity>(
            bool isSortableRequired,
            bool isFilterableRequired,
            string name)
        {
            if (!_options.MetadataSourceType.HasFlag(MetadataSourceType.FluentApi))
            {
                throw new InvalidOperationException(
                    $"Current {nameof(MetadataSourceType)} setting does not " +
                    $"allow support {nameof(MetadataSourceType.FluentApi)}. " +
                    $"Include {nameof(MetadataSourceType.FluentApi)} option to " +
                    $"be able to use it.");
            }

            var comparisonMethod = _options.IsCaseInsensitiveForNames
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;

            _propertyMetadata.TryGetValue(typeof(TEntity), out ISet<IPropertyMetadata> metadataSet);

            if (metadataSet == null)
            {
                return null;
            }

            return metadataSet.FirstOrDefault(metadata =>
            {
                return (metadata.DisplayName ?? metadata.Name).Equals(name, comparisonMethod)
                    && (!isSortableRequired || metadata.IsSortable)
                    && (!isFilterableRequired || metadata.IsFilterable);
            });
        }

        public IPropertyBuilder<TEntity> Property<TEntity>(Expression<Func<TEntity, object>> expression)
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

            return new PropertyBuilder<TEntity>(this, expression);
        }
    }
}
