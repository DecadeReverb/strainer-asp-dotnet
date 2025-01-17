﻿using Fluorite.Strainer.Models;
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
    public IStrainerModuleBuilder<T> AddCustomFilterMethod(Func<ICustomFilterMethodBuilder<T>, ICustomFilterMethod<T>> buildingDelegate)
    {
        Guard.Against.Null(buildingDelegate);

        var builder = new CustomFilterMethodBuilder<T>();
        var customMethod = buildingDelegate.Invoke(builder);

        if (!Module.CustomFilterMethods.ContainsKey(typeof(T)))
        {
            Module.CustomFilterMethods[typeof(T)] = new Dictionary<string, ICustomFilterMethod>();
        }

        Module.CustomFilterMethods[typeof(T)][customMethod.Name] = customMethod;

        return this;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">
    /// <paramref name="buildingDelegate"/> is <see langword="null"/>.
    /// </exception>
    public IStrainerModuleBuilder<T> AddCustomSortMethod(
        Func<ICustomSortMethodBuilder<T>, ICustomSortMethod<T>> buildingDelegate)
    {
        Guard.Against.Null(buildingDelegate);

        var builder = new CustomSortMethodBuilder<T>();
        var customMethod = buildingDelegate.Invoke(builder);

        if (!Module.CustomSortMethods.ContainsKey(typeof(T)))
        {
            Module.CustomSortMethods[typeof(T)] = new Dictionary<string, ICustomSortMethod>();
        }

        Module.CustomSortMethods[typeof(T)][customMethod.Name] = customMethod;

        return this;
    }

    /// <inheritdoc/>
    /// <exception cref="ArgumentException">
    /// <paramref name="buildingDelegate"/> is <see langword="null"/>.
    /// </exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="buildingDelegate"/> is used by already defined filter operator.
    /// </exception>
    public IStrainerModuleBuilder<T> AddFilterOperator(Func<IFilterOperatorBuilder, IFilterOperator> buildingDelegate)
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

    /// <inheritdoc/>
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

    /// <inheritdoc/>
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
