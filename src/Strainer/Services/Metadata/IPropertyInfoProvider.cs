﻿using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.Services.Metadata;

/// <summary>
/// Provides <see cref="PropertyInfo"/> and full member name when supplied
/// with an <see cref="Expression{TDelegate}"/>.
/// </summary>
public interface IPropertyInfoProvider
{
    PropertyInfo? GetPropertyInfo(Type type, string name);

    /// <summary>
    /// Gets the <see cref="PropertyInfo"/> and property full name
    /// (for nested property paths).
    /// </summary>
    /// <typeparam name="T">
    /// The base type expression is based on.
    /// </typeparam>
    /// <param name="expression">
    /// The lamda expression leading to property.
    /// </param>
    /// <returns>
    /// A tuple of <see cref="PropertyInfo"/> and <see cref="string"/>
    /// full property name.
    /// </returns>
    (PropertyInfo PropertyInfo, string FullName) GetPropertyInfoAndFullName<T>(
        Expression<Func<T, object>> expression);

    PropertyInfo[] GetPropertyInfos(Type type);
}
