using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Sorting;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Modules;

/// <summary>
/// Provides methods for configuring Strainer Module.
/// </summary>
/// <typeparam name="T">
/// The type of model for which this module builder is for.
/// </typeparam>
public interface IStrainerModuleBuilder<T>
{
    /// <summary>
    /// Gets the <see cref="StrainerOptions"/>.
    /// </summary>
    StrainerOptions Options { get; }

    /// <summary>
    /// Adds custom filtering method.
    /// </summary>
    /// <param name="name">
    /// The name for custom filtering method.
    /// </param>
    /// <returns>
    /// A builder instance for further configuration of custom filtering method.
    /// </returns>
    ICustomFilterMethodBuilder<T> AddCustomFilterMethod(string name);

    /// <summary>
    /// Adds custom sorting method.
    /// </summary>
    /// <param name="name">
    /// The name for custom sorting method.
    /// </param>
    /// <returns>
    /// A builder instance for further configuration of custom sorting method.
    /// </returns>
    ICustomSortMethodBuilder<T> AddCustomSortMethod(string name);

    /// <summary>
    /// Adds filter operator.
    /// </summary>
    /// <param name="symbol">
    /// The symbol for the filter operator.
    /// </param>
    /// <returns>
    /// A builder instance for further configuration of filter operator.
    /// </returns>
    IFilterOperatorBuilder AddFilterOperator(string symbol);

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
    IObjectMetadataBuilder<T> AddObject(
        Expression<Func<T, object>> defaultSortingPropertyExpression);

    /// <summary>
    /// Registers property metadata.
    /// </summary>
    /// <param name="propertyExpression">
    /// An expression leading to a property.
    /// </param>
    /// <returns>
    /// A builder instance for further configuration of property metadata.
    /// </returns>
    IPropertyMetadataBuilder<T> AddProperty(
        Expression<Func<T, object>> propertyExpression);

    /// <summary>
    /// Removes a filter operator.
    /// </summary>
    /// <param name="symbol">
    /// The symbol for the filter operator.
    /// </param>
    /// <returns>
    /// A builder instance for further configuration of Strainer module.
    /// </returns>
    IStrainerModuleBuilder<T> RemoveBuiltInFilterOperator(string symbol);
}
