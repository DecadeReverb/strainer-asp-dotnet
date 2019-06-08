namespace Fluorite.Strainer.Models.Filtering.Operators
{
    /// <summary>
    /// Represents starts with (case insensitive) filter operator.
    /// </summary>
    public class StartsWithCaseInsensitiveOperator : FilterOperator
    {
        /// <summary>
        /// Initializes new instance of <see cref="StartsWithCaseInsensitiveOperator"/> class.
        /// </summary>
        public StartsWithCaseInsensitiveOperator() : base(name: "starts with (case insensitive)", symbol: "_=*")
        {

        }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is case insensitive.
        /// </summary>
        public override bool IsCaseInsensitive => true;
    }
}
