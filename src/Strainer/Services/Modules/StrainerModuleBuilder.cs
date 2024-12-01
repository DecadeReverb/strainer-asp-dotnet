using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Sorting;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.Services.Modules;

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
        PropertyInfoProvider = Guard.Against.Null(propertyInfoProvider);
        Module = Guard.Against.Null(strainerModule);
        Options = Guard.Against.Null(strainerOptions);
    }

    /// <inheritdoc/>
    public StrainerOptions Options { get; }

    /// <summary>
    /// Gets the Strainer module on which this builder operates on.
    /// </summary>
    protected IStrainerModule Module { get; }

    /// <summary>
    /// Gets the property info provider.
    /// </summary>
    protected IPropertyInfoProvider PropertyInfoProvider { get; }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">
    /// <paramref name="buildingDelegate"/> is <see langword="null"/>.
    /// </exception>
    public IStrainerModuleBuilder AddCustomFilterMethod<TEntity>(
        Func<ICustomFilterMethodBuilder<TEntity>, ICustomFilterMethod<TEntity>> buildingDelegate)
    {
        Guard.Against.Null(buildingDelegate);

        var builder = new CustomFilterMethodBuilder<TEntity>();
        var customMethod = buildingDelegate.Invoke(builder);

        if (!Module.CustomFilterMethods.ContainsKey(typeof(TEntity)))
        {
            Module.CustomFilterMethods[typeof(TEntity)] = new Dictionary<string, ICustomFilterMethod>();
        }

        Module.CustomFilterMethods[typeof(TEntity)][customMethod.Name] = customMethod;

        return this;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">
    /// <paramref name="buildingDelegate"/> is <see langword="null"/>.
    /// </exception>
    public IStrainerModuleBuilder AddCustomSortMethod<TEntity>(
        Func<ICustomSortMethodBuilder<TEntity>, ICustomSortMethod<TEntity>> buildingDelegate)
    {
        Guard.Against.Null(buildingDelegate);

        var builder = new CustomSortMethodBuilder<TEntity>();
        var customMethod = buildingDelegate.Invoke(builder);

        if (!Module.CustomSortMethods.ContainsKey(typeof(TEntity)))
        {
            Module.CustomSortMethods[typeof(TEntity)] = new Dictionary<string, ICustomSortMethod>();
        }

        Module.CustomSortMethods[typeof(TEntity)][customMethod.Name] = customMethod;

        return this;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">
    /// <paramref name="buildingDelegate"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="buildingDelegate"/> is used by already defined filter operator.
    /// </exception>
    public IStrainerModuleBuilder AddFilterOperator(Func<IFilterOperatorBuilder, IFilterOperator> buildingDelegate)
    {
        Guard.Against.Null(buildingDelegate);

        var builder = new FilterOperatorBuilder();
        var filterOperator = buildingDelegate.Invoke(builder);

        if (Module.FilterOperators.Keys.Contains(filterOperator.Symbol))
        {
            throw new InvalidOperationException(
                $"There is an already existing operator with a symbol {filterOperator.Symbol}. " +
                $"Please, choose a different symbol.");
        }

        Module.FilterOperators.Add(filterOperator.Symbol, filterOperator);

        return this;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">
    /// <paramref name="defaultSortingPropertyExpression"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// Fluent API is not supported.
    /// </exception>
    public IObjectMetadataBuilder<TEntity> AddObject<TEntity>(
        Expression<Func<TEntity, object>> defaultSortingPropertyExpression)
    {
        Guard.Against.Null(defaultSortingPropertyExpression);

        if (!Options.MetadataSourceType.HasFlag(MetadataSourceType.FluentApi))
        {
            throw new NotSupportedException(
                $"Current {nameof(MetadataSourceType)} setting does not " +
                $"support {nameof(MetadataSourceType.FluentApi)}. " +
                $"Include {nameof(MetadataSourceType.FluentApi)} option to " +
                $"be able to use it.");
        }

        return new ObjectMetadataBuilder<TEntity>(
            PropertyInfoProvider,
            Module.ObjectMetadata,
            defaultSortingPropertyExpression);
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">
    /// <paramref name="propertyExpression"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="NotSupportedException">
    /// Fluent API is not supported.
    /// </exception>
    public IPropertyMetadataBuilder<TEntity> AddProperty<TEntity>(
        Expression<Func<TEntity, object>> propertyExpression)
    {
        Guard.Against.Null(propertyExpression);

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

    /// <inheritdoc/>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="symbol"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="symbol"/> is empty or whitespace.
    /// </exception>
    public IStrainerModuleBuilder RemoveBuiltInFilterOperator(string symbol)
    {
        Guard.Against.NullOrWhiteSpace(symbol);

        Module.ExcludedBuiltInFilterOperators.Add(symbol);

        return this;
    }
}
