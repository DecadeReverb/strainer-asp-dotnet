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
    /// <summary>
    /// Provides configuration information about metadata, filter operators
    /// and custom methods for Strainer.
    /// </summary>
    public abstract class StrainerModule
    {
        /// <summary>
        /// Initializes new instance of <see cref="StrainerModule"/> class.
        /// </summary>
        protected StrainerModule()
        {
            CustomFilterMethods = new Dictionary<Type, IDictionary<string, ICustomFilterMethod>>();
            CustomSortMethods = new Dictionary<Type, IDictionary<string, ICustomSortMethod>>();
            DefaultMetadata = new Dictionary<Type, IPropertyMetadata>();
            FilterOperators = new Dictionary<string, IFilterOperator>();
            PropertyMetadata = new Dictionary<Type, IDictionary<string, IPropertyMetadata>>();
            ObjectMetadata = new Dictionary<Type, IObjectMetadata>();
        }

        /// <summary>
        /// Gets the object custom filter methods dictionary.
        /// </summary>
        public IDictionary<Type, IDictionary<string, ICustomFilterMethod>> CustomFilterMethods { get; }

        /// <summary>
        /// Gets the object custom sorting methods dictionary.
        /// </summary>
        public IDictionary<Type, IDictionary<string, ICustomSortMethod>> CustomSortMethods { get; }

        /// <summary>
        /// Gets the object default dictionary.
        /// </summary>
        public IDictionary<Type, IPropertyMetadata> DefaultMetadata { get; }

        /// <summary>
        /// Gets the object filter operator dictionary.
        /// </summary>
        public IDictionary<string, IFilterOperator> FilterOperators { get; }

        /// <summary>
        /// Gets the object property dictionary.
        /// </summary>
        public IDictionary<Type, IDictionary<string, IPropertyMetadata>> PropertyMetadata { get; }

        /// <summary>
        /// Gets the object metadata dictionary.
        /// </summary>
        public IDictionary<Type, IObjectMetadata> ObjectMetadata { get; }

        /// <summary>
        /// Gets or sets the <see cref="StrainerOptions"/>.
        /// </summary>
        public StrainerOptions Options { get; set; }

        /// <summary>
        /// Adds custom filtering method.
        /// </summary>
        /// <typeparam name="TEntity">
        /// The type of entity on which custom filter method will operate.
        /// </typeparam>
        /// <param name="name">
        /// The name for custom filtering method.
        /// </param>
        /// <returns>
        /// A builder instance for further configuration of custom filtering method.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="name"/> is <see langword="null"/>, empty or
        /// contains only whitespace characters.
        /// </exception>
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

        /// <summary>
        /// Adds custom sorting method.
        /// </summary>
        /// <typeparam name="TEntity">
        /// The type of entity on which custom sorting method will operate.
        /// </typeparam>
        /// <param name="name">
        /// The name for custom sorting method.
        /// </param>
        /// <returns>
        /// A builder instance for further configuration of custom sorting method.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="name"/> is <see langword="null"/>, empty or
        /// contains only whitespace characters.
        /// </exception>
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

        /// <summary>
        /// Adds filter operator.
        /// </summary>
        /// <param name="symbol">
        /// The symbol for the filter operator.
        /// </param>
        /// <returns>
        /// A builder instance for further configuration of filter operator.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="symbol"/> is <see langword="null"/>, empty or
        /// contains only whitespace characters.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// <paramref name="symbol"/> is used by already defined filter operator.
        /// </exception>
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

        /// <summary>
        /// Registers object metadata.
        /// </summary>
        /// <typeparam name="TEntity">
        /// The type of object.
        /// </typeparam>
        /// <param name="defaultSortingPropertyExpression">
        /// An expression leading to a property marking default sorting.
        /// <para/>
        /// Default sorting property acts as a fallback when no other sorting
        /// information is available.
        /// </param>
        /// <returns>
        /// A builder instance for further configuration of object metadata.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="defaultSortingPropertyExpression"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Fluent API is not supported.
        /// </exception>
        public IObjectMetadataBuilder<TEntity> AddObject<TEntity>(
            Expression<Func<TEntity, object>> defaultSortingPropertyExpression)
        {
            if (defaultSortingPropertyExpression == null)
            {
                throw new ArgumentNullException(nameof(defaultSortingPropertyExpression));
            }

            if (!Options.MetadataSourceType.HasFlag(MetadataSourceType.FluentApi))
            {
                throw new NotSupportedException(
                    $"Current {nameof(MetadataSourceType)} setting does not " +
                    $"support {nameof(MetadataSourceType.FluentApi)}. " +
                    $"Include {nameof(MetadataSourceType.FluentApi)} option to " +
                    $"be able to use it.");
            }

            return new ObjectMetadataBuilder<TEntity>(ObjectMetadata, defaultSortingPropertyExpression);
        }

        /// <summary>
        /// Registers property metadata.
        /// </summary>
        /// <typeparam name="TEntity">
        /// The type of property owner, in other words type of object where
        /// property is defined.
        /// </typeparam>
        /// <param name="propertyExpression">
        /// An expression leading to a property.
        /// </param>
        /// <returns>
        /// A builder instance for further configuration of property metadata.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// <paramref name="propertyExpression"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// Fluent API is not supported.
        /// </exception>
        public IPropertyMetadataBuilder<TEntity> AddProperty<TEntity>(
            Expression<Func<TEntity, object>> propertyExpression)
        {
            if (propertyExpression == null)
            {
                throw new ArgumentNullException(nameof(propertyExpression));
            }

            if (!Options.MetadataSourceType.HasFlag(MetadataSourceType.FluentApi))
            {
                throw new NotSupportedException(
                    $"Current {nameof(MetadataSourceType)} setting does not " +
                    $"support {nameof(MetadataSourceType.FluentApi)}. " +
                    $"Include {nameof(MetadataSourceType.FluentApi)} option to " +
                    $"be able to use it.");
            }

            if (!PropertyMetadata.ContainsKey(typeof(TEntity)))
            {
                PropertyMetadata[typeof(TEntity)] = new Dictionary<string, IPropertyMetadata>();
            }

            return new PropertyMetadataBuilder<TEntity>(PropertyMetadata, DefaultMetadata, propertyExpression);
        }

        /// <summary>
        /// Loads configuration in module.
        /// <para/>
        /// Override this method to specify configuration for this module.
        /// </summary>
        public virtual void Load()
        {

        }
    }
}
