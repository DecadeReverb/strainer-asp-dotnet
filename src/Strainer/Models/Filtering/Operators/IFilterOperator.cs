﻿using System.Linq.Expressions;

namespace Fluorite.Strainer.Models.Filtering.Operators;

/// <summary>
/// Provides information about filtering operator.
/// </summary>
public interface IFilterOperator
{
    /// <summary>
    /// Gets a func providing an <see cref="System.Linq.Expressions.Expression"/>
    /// with filter operator applied when supplied a <see cref="IFilterExpressionContext"/>.
    /// </summary>
    Func<IFilterExpressionContext, Expression> Expression { get; }

    /// <summary>
    /// Gets a value indicating whether current
    /// operator is case insensitive.
    /// </summary>
    bool IsCaseInsensitive { get; }

    /// <summary>
    /// Gets a value indicating whether associated
    /// <see cref="System.Linq.Expressions.Expression"/> uses method
    /// based on a <see cref="string"/> instance like
    /// <see cref="string.Contains(string)"/> or <see cref="string.StartsWith(string)"/>.
    /// </summary>
    bool IsStringBased { get; }

    /// <summary>
    /// Gets the operator name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the <see cref="string"/> representation of operator.
    /// </summary>
    string Symbol { get; }
}
