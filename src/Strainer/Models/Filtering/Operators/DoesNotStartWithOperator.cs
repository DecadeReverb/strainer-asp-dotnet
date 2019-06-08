namespace Fluorite.Strainer.Models.Filtering.Operators
{
    /// <summary>
    /// Represents does not start with filter operator.
    /// </summary>
    public class DoesNotStartWithOperator : FilterOperator
    {
        /// <summary>
        /// Initializes new instance of <see cref="DoesNotStartWithOperator"/> class.
        /// </summary>
        public DoesNotStartWithOperator() : base(name: "does not start with", symbol: "!_=")
        {

        }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is a negated version of a different operator.
        /// </summary>
        public override bool IsNegated => true;
    }
}
