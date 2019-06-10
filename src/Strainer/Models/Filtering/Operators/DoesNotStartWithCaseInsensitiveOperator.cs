namespace Fluorite.Strainer.Models.Filtering.Operators
{
    /// <summary>
    /// Represents does not start with (case insensitive) filter operator.
    /// </summary>
    public class DoesNotStartWithCaseInsensitiveOperator : FilterOperator
    {
        /// <summary>
        /// Initializes new instance of <see cref="DoesNotStartWithCaseInsensitiveOperator"/> class.
        /// </summary>
        public DoesNotStartWithCaseInsensitiveOperator() : base(name: "does not start with (case insensitive)", symbol: "!_=*")
        {

        }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is case insensitive.
        /// </summary>
        public override bool IsCaseInsensitive => true;

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is a negated version of a different operator.
        /// </summary>
        public override bool IsNegated => true;
    }
}
