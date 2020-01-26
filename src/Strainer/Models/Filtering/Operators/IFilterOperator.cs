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
        /// Gets a <see cref="bool"/> value indictating whether associated
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
}
