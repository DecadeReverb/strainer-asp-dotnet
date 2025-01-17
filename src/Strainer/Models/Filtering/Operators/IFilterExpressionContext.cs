﻿using System.Linq.Expressions;

namespace Fluorite.Strainer.Models.Filtering.Operators;

/// <summary>
/// Represents information context for filter expression.
/// </summary>
public interface IFilterExpressionContext
{
    /// <summary>
    /// Gets the expression for filter value being processed.
    /// </summary>
    Expression FilterValue { get; }

    /// <summary>
    /// Gets the expression for property value access.
    /// </summary>
    Expression PropertyValue { get; }
}
