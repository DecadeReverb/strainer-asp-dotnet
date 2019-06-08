namespace Fluorite.Strainer.Models.Filtering.Operators
{
    /// <summary>
    /// Represents contains (case insensitive) filter operator.
    /// </summary>
    public class ContainsCaseInsensitiveOperator : FilterOperator
    {
        /// <summary>
        /// Initializes new instance of <see cref="ContainsCaseInsensitiveOperator"/> class.
        /// </summary>
        public ContainsCaseInsensitiveOperator() : base(name: "contains (case insensitive)", symbol: "@=*")
        {

        }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is case insensitive.
        /// </summary>
        public override bool IsCaseInsensitive => true;

    }
}
