namespace Fluorite.Strainer.Models.Filtering.Operators
{
    /// <summary>
    /// Represents less than filter operator.
    /// </summary>
    public class LessThanOrEqualToOperator : FilterOperator
    {
        /// <summary>
        /// Initializes new instance of <see cref="LessThanOrEqualToOperator"/> class.
        /// </summary>
        public LessThanOrEqualToOperator() : base(name: "less than or equal to", symbol: "<=")
        {

        }
    }
}
