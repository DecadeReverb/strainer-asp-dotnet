namespace Fluorite.Strainer.Models.Filtering.Operators
{
    /// <summary>
    /// Represents does not contain filter operator.
    /// </summary>
    public class DoesNotContainOperator : FilterOperator
    {
        /// <summary>
        /// Initializes new instance of <see cref="DoesNotContainOperator"/> class.
        /// </summary>
        public DoesNotContainOperator() : base(name: "does not contain", symbol: "!@=")
        {

        }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is a negated version of a different operator.
        /// </summary>
        public override bool IsNegated => true;
    }
}
