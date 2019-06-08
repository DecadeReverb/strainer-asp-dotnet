namespace Fluorite.Strainer.Models.Filtering.Operators
{
    /// <summary>
    /// Represents greater than or equal to filter operator.
    /// </summary>
    public class GreaterThanOrEqualToOperator : FilterOperator
    {
        /// <summary>
        /// Initializes new instance of <see cref="GreaterThanOrEqualToOperator"/> class.
        /// </summary>
        public GreaterThanOrEqualToOperator() : base(name: "greater than or equal to", symbol: ">=")
        {

        }
    }
}
