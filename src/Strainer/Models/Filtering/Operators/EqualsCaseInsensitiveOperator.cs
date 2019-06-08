namespace Fluorite.Strainer.Models.Filtering.Operators
{
    /// <summary>
    /// Represents equals (case insensitive) filter operator.
    /// </summary>
    public class EqualsCaseInsensitiveOperator : FilterOperator
    {
        /// <summary>
        /// Initializes new instance of <see cref="EqualsCaseInsensitiveOperator"/> class.
        /// </summary>
        public EqualsCaseInsensitiveOperator() : base(name: "equals (case insensitive)", symbol: "==*")
        {

        }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is case insensitive.
        /// </summary>
        public override bool IsCaseInsensitive => true;
    }
}
