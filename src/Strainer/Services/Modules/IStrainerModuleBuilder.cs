using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Sorting;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Services.Modules;

/// <summary>
/// Provides methods for configuring Strainer Module.
/// </summary>
public interface IStrainerModuleBuilder
{
    /// <summary>
    /// Gets the <see cref="StrainerOptions"/>.
    /// </summary>
    StrainerOptions Options { get; }

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
    ICustomFilterMethodBuilder<TEntity> AddCustomFilterMethod<TEntity>(string name);

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
    ICustomSortMethodBuilder<TEntity> AddCustomSortMethod<TEntity>(string name);

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
    IObjectMetadataBuilder<TEntity> AddObject<TEntity>(
        Expression<Func<TEntity, object>> defaultSortingPropertyExpression);

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
    IPropertyMetadataBuilder<TEntity> AddProperty<TEntity>(
        Expression<Func<TEntity, object>> propertyExpression);

    /// <summary>
    /// Removes a filter operator.
    /// </summary>
    /// <param name="symbol">
    /// The symbol for the filter operator.
    /// </param>
    /// <returns>
    /// A builder instance for further configuration of Strainer module.
    /// </returns>
    IStrainerModuleBuilder RemoveBuiltInFilterOperator(string symbol);
}
