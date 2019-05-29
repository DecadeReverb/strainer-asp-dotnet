namespace Sieve.Models.Filtering.Operators
{
    /// <summary>
    /// Represents equals filter operator.
    /// </summary>
    public class EqualsOperator : FilterOperator
    {
        /// <summary>
        /// Initializes new instance of <see cref="EqualsOperator"/> class.
        /// </summary>
        public EqualsOperator() : base(name: "equals", @operator: "==")
        {

        }
    }
}
