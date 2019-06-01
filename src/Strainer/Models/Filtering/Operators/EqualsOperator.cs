namespace Strainer.Models.Filtering.Operators
{
    /// <summary>
    /// Represents equals filter operator.
    /// </summary>
    public class EqualsOperator : FilterOperator
    {
        /// <summary>
        /// Initializes new instance of <see cref="EqualsOperator"/> class.
        /// </summary>
        public EqualsOperator() : base(name: "equals", @operator: "==")
        {

        }

        /// <summary>
        /// Gets a <see cref="bool"/> value indictating whether current
        /// operator is a default operator.
        /// </summary>
        public override bool IsDefault => true;
    }
}
