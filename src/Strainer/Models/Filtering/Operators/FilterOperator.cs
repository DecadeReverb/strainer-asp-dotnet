using System;
using System.Diagnostics;

namespace Fluorite.Strainer.Models.Filtering.Operators
{
    /// <summary>
    /// Represents filter operator.
    /// </summary>
    [DebuggerDisplay("{Operator,nq} {Name,nq}")]
    public abstract class FilterOperator : IFilterOperator/*, IEquatable<FilterOperator>*/
    {
        /// <summary>
        /// Initializes new instance of <see cref="FilterOperator"/> class
        /// with name and operator.
        /// </summary>
        /// <param name="name">
        /// The name of filter operator.
        /// </param>
        /// <param name="operator">
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
        protected FilterOperator(string name, string @operator)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentException(
                    $"{nameof(name)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(name));
            }

            if (string.IsNullOrWhiteSpace(@operator))
            {
                throw new ArgumentException(
                    $"{nameof(@operator)} cannot be null, empty " +
                    $"or contain only whitespace characaters.",
                    nameof(@operator));
            }

            Name = name;
            Operator = @operator;
        }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is case insensitive
        /// </summary>
        public virtual bool IsCaseInsensitive { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is a default operator.
        /// </summary>
        public virtual bool IsDefault { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is a negated version of different operator.
        /// </summary>
        public virtual bool IsNegated { get; }

        /// <summary>
        /// Gets the operator name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the <see cref="string"/> representation of operator.
        /// </summary>
        public string Operator { get; }

        /// <summary>
        /// Returns a <see cref="string"/> that represents the current <see cref="FilterOperator"/>.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents the current <see cref="FilterOperator"/>.
        /// </returns>
        public override string ToString()
        {
            return $"{Operator} {Name}";
        }
    }
}
