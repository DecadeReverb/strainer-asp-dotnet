namespace Fluorite.Strainer.Models.Filtering.Operators
{
    /// <summary>
    /// Defines minimum requirements for filter operator.
    /// </summary>
    public interface IFilterOperator
    {
        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is case insensitive
        /// </summary>
        bool IsCaseInsensitive { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is a default operator.
        /// </summary>
        bool IsDefault { get; }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is a negated version of different operator.
        /// </summary>
        bool IsNegated { get; }

        /// <summary>
        /// Gets the operator name.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Gets the <see cref="string"/> representation of operator.
        /// </summary>
        string Operator { get; }
    }
}
