using System;
using System.Diagnostics;

namespace Fluorite.Strainer.Models.Filtering.Operators
{
    /// <summary>
    /// Represents base filter operator.
    /// </summary>
    [DebuggerDisplay("{" + nameof(Symbol) + ",nq} {" + nameof(Name) + ",nq}")]
    public abstract class FilterOperator : IFilterOperator, IEquatable<FilterOperator>
    {
        /// <summary>
        /// Initializes new instance of <see cref="FilterOperator"/> class
        /// with name and operator.
        /// </summary>
        /// <param name="name">
        /// The name of filter operator.
        /// </param>
        /// <param name="symbol">
        /// <see cref="string"/> representation of the operator.
        /// </param>
        /// <exception cref="ArgumentException">
        /// <paramref name="name"/> is <see langword="null"/>, empty
        /// or contains only whitespace characters.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="@operator"/> is <see langword="null"/>, empty
        /// or contains only whitespace characters.
        /// </exception>
        protected FilterOperator(string name, string symbol)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    $"{nameof(name)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(name));
            }

            if (string.IsNullOrWhiteSpace(symbol))
            {
                throw new ArgumentException(
                    $"{nameof(symbol)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(symbol));
            }

            Name = name;
            Symbol = symbol;
        }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is case insensitive.
        /// </summary>
        public virtual bool IsCaseInsensitive { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is a default operator.
        /// </summary>
        public virtual bool IsDefault { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is a negated version of a different operator.
        /// </summary>
        public virtual bool IsNegated { get; }

        /// <summary>
        /// Gets the operator name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the <see cref="string"/> representation of operator.
        /// </summary>
        public string Symbol { get; }

        /// <summary>
        /// Checks if current instance of <see cref="FilterOperator"/> is equal
        /// to other <see cref="FilterOperator"/> instance.
        /// </summary>
        /// <param name="other">
        /// Other <see cref="FilterOperator"/> instance.
        /// </param>
        /// <returns>
        /// <see langword="true"/> if provided other <see cref="object"/>
        /// instance is equal to the current one; otherwise <see langword="false"/>.
        /// </returns>
        public bool Equals(FilterOperator other)
        {
            return other != null
                && Symbol.Equals(other.Symbol, StringComparison.OrdinalIgnoreCase);
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
    }
}
