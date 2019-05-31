namespace Sieve.Models
{
    /// <summary>
    /// Defines minimum requirements for filter operator.
    /// </summary>
    public interface IFilterOperator
    {
        ///// <summary>
        ///// Gets a <see cref="IFilterOperator"/> which is a not negated
        ///// version of current operator, if such exists; otherwise <see langword="null"/>.
        ///// </summary>
        //IFilterOperator CaseSensitiveVersion { get; }

        ///// <summary>
        ///// Gets a <see cref="bool"/> value indictating whether current
        ///// operator has a case sensitive version.
        ///// </summary>
        //bool HasCaseSensitiveVersion { get; }

        ///// <summary>
        ///// Gets a <see cref="bool"/> value indictating whether current
        ///// operator has a not negated version.
        ///// </summary>
        //bool HasUnnegatedVersion { get; }

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

        ///// <summary>
        ///// Gets a <see cref="IFilterOperator"/> which is a not negated
        ///// version of current operator, if such exists; otherwise <see langword="null"/>.
        ///// </summary>
        //IFilterOperator UnnegatedVersion { get; }
    }
}
