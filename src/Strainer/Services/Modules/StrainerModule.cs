using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Sorting;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Modules
{
    public abstract class StrainerModule
    {
        protected StrainerModule()
        {
            CustomFilterMethods = new Dictionary<Type, IDictionary<string, ICustomFilterMethod>>();
            CustomSortMethods = new Dictionary<Type, IDictionary<string, ICustomSortMethod>>();
            DefaultMetadata = new Dictionary<Type, IPropertyMetadata>();
            FilterOperators = new Dictionary<string, IFilterOperator>();
            PropertyMetadata = new Dictionary<Type, IDictionary<string, IPropertyMetadata>>();
            ObjectMetadata = new Dictionary<Type, IObjectMetadata>();
        }

        public IDictionary<Type, IDictionary<string, ICustomFilterMethod>> CustomFilterMethods { get; }

        public IDictionary<Type, IDictionary<string, ICustomSortMethod>> CustomSortMethods { get; }

        public IDictionary<Type, IPropertyMetadata> DefaultMetadata { get; }

        public IDictionary<string, IFilterOperator> FilterOperators { get; }

        public IDictionary<Type, IDictionary<string, IPropertyMetadata>> PropertyMetadata { get; }

        public IDictionary<Type, IObjectMetadata> ObjectMetadata { get; }

        public StrainerOptions Options { get; set; }

        public ICustomFilterMethodBuilder<TEntity> AddCustomFilterMethod<TEntity>(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    $"{nameof(name)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(name));
            }

            if (!CustomFilterMethods.ContainsKey(typeof(TEntity)))
            {
                CustomFilterMethods[typeof(TEntity)] = new Dictionary<string, ICustomFilterMethod>();
            }

            return new CustomFilterMethodBuilder<TEntity>(CustomFilterMethods, name);
        }

        public ICustomSortMethodBuilder<TEntity> AddCustomSortMethod<TEntity>(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    $"{nameof(name)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(name));
            }

            if (!CustomSortMethods.ContainsKey(typeof(TEntity)))
            {
                CustomSortMethods[typeof(TEntity)] = new Dictionary<string, ICustomSortMethod>();
            }

            return new CustomSortMethodBuilder<TEntity>(CustomSortMethods, name);
        }

        public IFilterOperatorBuilder AddFilterOperator(string symbol)
        {
            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException(
                    $"{nameof(symbol)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(symbol));
            }

            if (FilterOperators.Keys.Contains(symbol))
            {
                throw new InvalidOperationException(
                    $"There is an already existing operator with a symbol {symbol}. " +
                    $"Please, choose a different symbol.");
            }

            return new FilterOperatorBuilder(FilterOperators, symbol);
        }

        public IObjectMetadataBuilder<TEntity> AddObject<TEntity>(
            Expression<Func<TEntity, object>> defaultSortingPropertyExpression)
        {
            if (defaultSortingPropertyExpression == null)
            {
                throw new ArgumentNullException(nameof(defaultSortingPropertyExpression));
            }

            if (!Options.MetadataSourceType.HasFlag(MetadataSourceType.FluentApi))
            {
                throw new InvalidOperationException(
                    $"Current {nameof(MetadataSourceType)} setting does not " +
                    $"allow support {nameof(MetadataSourceType.FluentApi)}. " +
                    $"Include {nameof(MetadataSourceType.FluentApi)} option to " +
                    $"be able to use it.");
            }

            return new ObjectMetadataBuilder<TEntity>(ObjectMetadata, defaultSortingPropertyExpression);
        }

        public IPropertyMetadataBuilder<TEntity> AddProperty<TEntity>(
            Expression<Func<TEntity, object>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            if (!Options.MetadataSourceType.HasFlag(MetadataSourceType.FluentApi))
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

        public virtual void Load()
        {

        }
    }
}
