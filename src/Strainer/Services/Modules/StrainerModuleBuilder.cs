using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Sorting;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.Services.Modules
{
    /// <summary>
    /// Default implementation of Strainer Module builder service.
    /// </summary>
    public class StrainerModuleBuilder : IStrainerModuleBuilder
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StrainerModuleBuilder"/>
        /// class.
        /// </summary>
        /// <param name="strainerModule">
        /// The Strainer moduel to operate on.
        /// </param>
        /// <param name="strainerOptions">
        /// The Stariner options.
        /// </param>
        /// <param name="propertyInfoProvider">
        /// The <see cref="PropertyInfo"/> provider.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="strainerModule"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="strainerOptions"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="propertyInfoProvider"/> is <see langword="null"/>.
        /// </exception>
        public StrainerModuleBuilder(
            IPropertyInfoProvider propertyInfoProvider,
            IStrainerModule strainerModule,
            StrainerOptions strainerOptions)
        {
            PropertyInfoProvider = propertyInfoProvider ?? throw new ArgumentNullException(nameof(propertyInfoProvider));
            Module = strainerModule ?? throw new ArgumentNullException(nameof(strainerModule));
            Options = strainerOptions ?? throw new ArgumentNullException(nameof(strainerOptions));
        }

        /// <summary>
        /// Gets the <see cref="StrainerOptions"/>.
        /// </summary>
        public StrainerOptions Options { get; }

        /// <summary>
        /// Gets the Strainer module on which this builder operates on.
        /// </summary>
        protected IStrainerModule Module { get; }

        /// <summary>
        /// Gets the property info provider.
        /// </summary>
        protected IPropertyInfoProvider PropertyInfoProvider { get; }

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
                    $"or contain only whitespace characters.",
                    nameof(name));
            }

            if (!Module.CustomFilterMethods.ContainsKey(typeof(TEntity)))
            {
                Module.CustomFilterMethods[typeof(TEntity)] = new Dictionary<string, ICustomFilterMethod>();
            }

            return new CustomFilterMethodBuilder<TEntity>(Module.CustomFilterMethods, name);
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
                    $"or contain only whitespace characters.",
                    nameof(name));
            }

            if (!Module.CustomSortMethods.ContainsKey(typeof(TEntity)))
            {
                Module.CustomSortMethods[typeof(TEntity)] = new Dictionary<string, ICustomSortMethod>();
            }

            return new CustomSortMethodBuilder<TEntity>(Module.CustomSortMethods, name);
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
                    $"or contain only whitespace characters.",
                    nameof(symbol));
            }

            if (Module.FilterOperators.Keys.Contains(symbol))
            {
                throw new InvalidOperationException(
                    $"There is an already existing operator with a symbol {symbol}. " +
                    $"Please, choose a different symbol.");
            }

            return new FilterOperatorBuilder(Module.FilterOperators, symbol);
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

            return new ObjectMetadataBuilder<TEntity>(Module.ObjectMetadata, defaultSortingPropertyExpression);
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

            if (!Module.PropertyMetadata.ContainsKey(typeof(TEntity)))
            {
                Module.PropertyMetadata[typeof(TEntity)] = new Dictionary<string, IPropertyMetadata>();
            }

            var (propertyInfo, fullName) = PropertyInfoProvider.GetPropertyInfoAndFullName(propertyExpression);

            return new PropertyMetadataBuilder<TEntity>(Module.PropertyMetadata, Module.DefaultMetadata, propertyInfo, fullName);
        }
    }
}
