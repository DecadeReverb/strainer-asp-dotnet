namespace Sieve.Models.Filtering.Operators
{
    /// <summary>
    /// Represents not equals filter operator.
    /// </summary>
    public class NotEqualsOperator : FilterOperator
    {
        /// <summary>
        /// Initializes new instance of <see cref="NotEqualsOperator"/> class.
        /// </summary>
        public NotEqualsOperator() : base(name: "not equals", @operator: "!=")
        {

        }
    }
}
