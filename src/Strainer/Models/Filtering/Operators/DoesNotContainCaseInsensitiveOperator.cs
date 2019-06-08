namespace Fluorite.Strainer.Models.Filtering.Operators
{
    /// <summary>
    /// Represents does not contain (case insensitive) filter operator.
    /// </summary>
    public class DoesNotContainCaseInsensitiveOperator : FilterOperator
    {
        /// <summary>
        /// Initializes new instance of <see cref="DoesNotContainCaseInsensitiveOperator"/>
        /// class.
        /// </summary>
        public DoesNotContainCaseInsensitiveOperator() : base(name: "does not contain (case insensitive)", symbol: "!@=*")
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
