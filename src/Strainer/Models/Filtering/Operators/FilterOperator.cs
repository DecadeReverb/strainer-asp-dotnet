using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq.Expressions;

namespace Fluorite.Strainer.Models.Filtering.Operators
{
    /// <summary>
    /// Provides information about filtering operator.
    /// </summary>
    [DebuggerDisplay("{" + nameof(Symbol) + ",nq} {" + nameof(Name) + ",nq}")]
    public class FilterOperator : IFilterOperator, IEquatable<FilterOperator>
    {
        /// <summary>
        /// Initializes new instance of <see cref="FilterOperator"/> class.
        /// </summary>
        public FilterOperator()
        {

        }

        /// <summary>
        /// Gets or sets a function leading to <see cref="System.Linq.Expressions.Expression"/>
        /// associated to current operator.
        /// </summary>
        public Func<IFilterExpressionContext, Expression> Expression { get; set;  }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indictating whether current
        /// operator is case insensitive.
        /// </summary>
        public bool IsCaseInsensitive { get; set; }

        /// <summary>
        /// Gets or sets <see cref="bool"/> value indictating whether associated
        /// <see cref="System.Linq.Expressions.Expression"/> uses method
        /// based on a <see cref="string"/> instance like
        /// <see cref="string.Contains(string)"/> or <see cref="string.StartsWith(string)"/>.
        /// </summary>
        public bool IsStringBased { get; set; }

        /// <summary>
        /// Gets or sets the operator name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="string"/> representation of the operator.
        /// </summary>
        public string Symbol { get; set; }

        /// <summary>
        /// Checks if current instance of <see cref="FilterOperator"/>
        /// is equal to other <see cref="object"/> instance.
        /// </summary>
        /// <param name="obj">
        /// Other <see cref="object"/> instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if provided other <see cref="object"/>
        /// instance is equal to the current one; otherwise <see langword="false"/>.
        /// </returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as FilterOperator);
        }

        /// <summary>
        /// Checks if current instance of <see cref="FilterOperator"/> is equal
        /// to other <see cref="FilterOperator"/> instance.
        /// </summary>
        /// <param name="other">
        /// Other <see cref="FilterOperator"/> instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if provided other <see cref="FilterOperator"/>
        /// instance is equal to the current one; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(FilterOperator other)
        {
            return other != null
                && Symbol.Equals(other.Symbol, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets <see cref="int"/> hash code representation of current
        /// <see cref="FilterOperator"/>.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="FilterOperator"/>.
        /// </returns>
        public override int GetHashCode()
        {
            return -1758840423 + EqualityComparer<string>.Default.GetHashCode(Symbol);
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="FilterOperator"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents the current <see cref="FilterOperator"/>.
        /// </returns>
        public override string ToString()
        {
            return $"{Symbol} {Name}";
        }

        public static bool operator ==(FilterOperator operator1, FilterOperator operator2)
        {
            return EqualityComparer<FilterOperator>.Default.Equals(operator1, operator2);
        }

        public static bool operator !=(FilterOperator operator1, FilterOperator operator2)
        {
            return !(operator1 == operator2);
        }
    }
}
