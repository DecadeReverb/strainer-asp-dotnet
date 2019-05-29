namespace Sieve.Models.Filtering.Operators
{
    /// <summary>
    /// Represents less than filter operator.
    /// </summary>
    public class LessThanOperator : FilterOperator
    {
        /// <summary>
        /// Initializes new instance of <see cref="LessThanOperator"/> class.
        /// </summary>
        public LessThanOperator() : base(name: "less than", @operator: "<")
        {

        }
    }
}
