namespace Sieve.Models
{
    /// <summary>
    /// Defines minimum requirements for filter operator.
    /// </summary>
    public interface IFilterOperator
    {
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
