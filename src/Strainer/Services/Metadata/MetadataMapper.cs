using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Metadata;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Metadata
{
    public class MetadataMapper : IMetadataMapper
    {
        private readonly StrainerOptions _options;

        public MetadataMapper(IStrainerOptionsProvider optionsProvider)
        {
            DefaultMetadata = new Dictionary<Type, IPropertyMetadata>();
            PropertyMetadata = new Dictionary<Type, IDictionary<string, IPropertyMetadata>>();
            ObjectMetadata = new Dictionary<Type, IObjectMetadata>();
            _options = (optionsProvider ?? throw new ArgumentNullException(nameof(optionsProvider)))
                .GetStrainerOptions();
        }

        public IDictionary<Type, IPropertyMetadata> DefaultMetadata { get; }

        public IDictionary<Type, IDictionary<string, IPropertyMetadata>> PropertyMetadata { get; }

        public IDictionary<Type, IObjectMetadata> ObjectMetadata { get; }

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

            ObjectMetadata[typeof(TEntity)] = objectMetadata;
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

            if (!PropertyMetadata.ContainsKey(typeof(TEntity)))
            {
                PropertyMetadata[typeof(TEntity)] = new Dictionary<string, IPropertyMetadata>();
            }

            if (propertyMetadata.IsDefaultSorting)
            {
                DefaultMetadata[typeof(TEntity)] = propertyMetadata;
            }

            var metadataKey = propertyMetadata.DisplayName ?? propertyMetadata.Name;

            PropertyMetadata[typeof(TEntity)][metadataKey] = propertyMetadata;
        }

        public IObjectMetadataBuilder<TEntity> Object<TEntity>(Expression<Func<TEntity, object>> defaultSortingPropertyExpression)
        {
            if (defaultSortingPropertyExpression == null)
            {
                throw new ArgumentNullException(nameof(defaultSortingPropertyExpression));
            }

            if (!_options.MetadataSourceType.HasFlag(MetadataSourceType.FluentApi))
            {
                throw new InvalidOperationException(
                    $"Current {nameof(MetadataSourceType)} setting does not " +
                    $"allow support {nameof(MetadataSourceType.FluentApi)}. " +
                    $"Include {nameof(MetadataSourceType.FluentApi)} option to " +
                    $"be able to use it.");
            }

            return new ObjectMetadataBuilder<TEntity>(ObjectMetadata, defaultSortingPropertyExpression);
        }

        public IPropertyMetadataBuilder<TEntity> Property<TEntity>(Expression<Func<TEntity, object>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            if (!_options.MetadataSourceType.HasFlag(MetadataSourceType.FluentApi))
            {
                throw new InvalidOperationException(
                    $"Current {nameof(MetadataSourceType)} setting does not " +
                    $"allow support {nameof(MetadataSourceType.FluentApi)}. " +
                    $"Include {nameof(MetadataSourceType.FluentApi)} option to " +
                    $"be able to use it.");
            }

            if (!PropertyMetadata.ContainsKey(typeof(TEntity)))
            {
                PropertyMetadata[typeof(TEntity)] = new Dictionary<string, IPropertyMetadata>();
            }

            return new PropertyMetadataBuilder<TEntity>(PropertyMetadata, DefaultMetadata, propertyExpression);
        }
    }
}
