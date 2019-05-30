using System;
using System.Diagnostics;

namespace Sieve.Models
{
    /// <summary>
    /// Represents filter operator.
    /// </summary>
    [DebuggerDisplay("{Operator,nq} {Name,nq}")]
    public class FilterOperator : IFilterOperator/*, IEquatable<FilterOperator>*/
    {
        /// <summary>
        /// Initializes new instance of <see cref="FilterOperator"/> class.
        /// </summary>
        public FilterOperator()
        {

        }

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
        public FilterOperator(string name, string @operator)
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
        /// Gets a <see cref="IFilterOperator"/> which is a not negated
        /// version of current operator, if such exists; otherwise <see langword="null"/>.
        /// </summary>
        public IFilterOperator CaseSensitiveVersion { get; set;  }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator has a case sensitive version.
        /// </summary>
        public bool HasCaseSensitiveVersion => CaseSensitiveVersion != null;

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator has a not negated version.
        /// </summary>
        public bool HasUnnegatedVersion => UnnegatedVersion != null;

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is case insensitive
        /// </summary>
        public bool IsCaseInsensitive { get; set;  }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is a default operator.
        /// </summary>
        public bool IsDefault { get; set; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is a negated version of different operator.
        /// </summary>
        public bool IsNegated { get; set; }

        /// <summary>
        /// Gets the operator name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets the <see cref="string"/> representation of operator.
        /// </summary>
        public string Operator { get; set; }

        /// <summary>
        /// Gets a <see cref="IFilterOperator"/> which is a not negated
        /// version of current operator, if such exists; otherwise <see langword="null"/>.
        /// </summary>
        public IFilterOperator UnnegatedVersion { get; set; }

        ///// <summary>
        ///// Determines whether the specified <see cref="object"/>
        ///// is equal to the current <see cref="FilterOperator"/>.
        ///// </summary>
        ///// <param name="obj">
        ///// The <see cref="object"/> to compare with the current <see cref="FilterOperator"/>.
        ///// </param>
        ///// <returns>
        ///// <see langword="true"/> if the specified object is equal
        ///// to the current <see cref="object"/>; otherwise, <see langword="false"/>.
        ///// </returns>
        //public override bool Equals(object obj)
        //{
        //    return Equals(obj as FilterOperator);
        //}

        ///// <summary>
        ///// Indicates whether the current <see cref="FilterOperator"/>
        ///// is equal to another <see cref="FilterOperator"/>.
        ///// </summary>
        ///// <param name="other">
        ///// An <see cref="FilterOperator"/> to compare with this <see cref="FilterOperator"/>.
        ///// </param>
        ///// <returns>
        ///// <see langword="true"/> if the specified object is equal
        ///// to the current <see cref="object"/>; otherwise, <see langword="false"/>.
        ///// </returns>
        //public bool Equals(FilterOperator other)
        //{
        //    return other != null && Operator == other.Operator;
        //}

        ///// <summary>
        ///// Gets hashcode representation of current <see cref="FilterOperator"/>
        ///// as <see cref="int"/>.
        ///// </summary>
        ///// <returns>
        ///// <see cref="int"/> hashcode.
        ///// </returns>
        //public override int GetHashCode()
        //{
        //    return -2036482651 + EqualityComparer<string>.Default.GetHashCode(Operator);
        //}

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

        //public static bool operator ==(FilterOperator operator1, FilterOperator operator2)
        //{
        //    return EqualityComparer<FilterOperator>.Default.Equals(operator1, operator2);
        //}

        //public static bool operator !=(FilterOperator operator1, FilterOperator operator2)
        //{
        //    return !(operator1 == operator2);
        //}
    }
}
