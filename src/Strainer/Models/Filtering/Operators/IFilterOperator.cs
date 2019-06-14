using System;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Models.Filtering.Operators
{
    /// <summary>
    /// Provides information about filtering symbol.
    /// </summary>
    public interface IFilterOperator
    {
        /// <summary>
        /// Gets a function leading to <see cref="System.Linq.Expressions.Expression"/>
        /// associated to current operator.
        /// </summary>
        Func<IFilterExpressionContext, Expression> Expression { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is case insensitive.
        /// </summary>
        bool IsCaseInsensitive { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is a default operator.
        /// </summary>
        bool IsDefault { get; }

        /// <summary>
        /// Gets the operator name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether associated
        /// <see cref="System.Linq.Expressions.Expression"/> should be negated
        /// before using it for filtering data.
        /// </summary>
        bool NegateExpression { get; }

        /// <summary>
        /// Gets the <see cref="string"/> representation of operator.
        /// </summary>
        string Symbol { get; }
    }
}
