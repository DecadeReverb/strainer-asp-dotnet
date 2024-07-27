using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Sorting;
using System.Linq.Expressions;
using System.Reflection;
using System.Xml.Linq;

namespace Fluorite.Strainer.Services.Modules;

/// <summary>
/// Default implementation of Strainer Module builder service.
/// </summary>
/// <typeparam name="T">
/// The type of model for which this module builder is for.
/// </typeparam>
public class StrainerModuleBuilder<T> : IStrainerModuleBuilder<T>
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StrainerModuleBuilder{T}"/>
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
    public ICustomFilterMethodBuilder<T> AddCustomFilterMethod(string name)
    {
        Guard.Against.NullOrWhiteSpace(name);

        return new CustomFilterMethodBuilder<T>(
            Module.CustomFilterMethods,
            name);
    }

    /// <summary>
    /// Adds custom sorting method.
    /// </summary>
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
    public ICustomSortMethodBuilder<T> AddCustomSortMethod(string name)
    {
        Guard.Against.NullOrWhiteSpace(name);

        return new CustomSortMethodBuilder<T>(
            Module.CustomSortMethods,
            name);
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
        Guard.Against.NullOrWhiteSpace(symbol);

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
    public IObjectMetadataBuilder<T> AddObject(
        Expression<Func<T, object>> defaultSortingPropertyExpression)
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

        return new ObjectMetadataBuilder<T>(
            PropertyInfoProvider,
            Module.ObjectMetadata,
            defaultSortingPropertyExpression);
    }

    /// <summary>
    /// Registers property metadata.
    /// </summary>
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
    public IPropertyMetadataBuilder<T> AddProperty(
        Expression<Func<T, object>> propertyExpression)
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

        var (propertyInfo, fullName) = PropertyInfoProvider.GetPropertyInfoAndFullName(propertyExpression);

        return new PropertyMetadataBuilder<T>(
            Module.PropertyMetadata,
            Module.DefaultMetadata,
            propertyInfo,
            fullName);
    }

    /// <summary>
    /// Removes a filter operator.
    /// </summary>
    /// <param name="symbol">
    /// The symbol for the filter operator.
    /// </param>
    /// <returns>
    /// A builder instance for further configuration of Strainer module.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="symbol"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="ArgumentException">
    /// <paramref name="symbol"/> is empty or whitespace.
    /// </exception>
    public IStrainerModuleBuilder<T> RemoveBuiltInFilterOperator(string symbol)
    {
        Guard.Against.NullOrWhiteSpace(symbol);

        Module.ExcludedBuiltInFilterOperators.Remove(symbol);

        return this;
    }
}
