namespace Fluorite.Strainer.Models.Filtering.Operators
{
    /// <summary>
    /// Represents not equals filter operator.
    /// </summary>
    public class NotEqualsOperator : FilterOperator
    {
        /// <summary>
        /// Initializes new instance of <see cref="NotEqualsOperator"/> class.
        /// </summary>
        public NotEqualsOperator() : base(name: "not equals", symbol: "!=")
        {

        }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is a negated version of different operator.
        /// </summary>
        public override bool IsNegated => true;
    }
}
