namespace Fluorite.Strainer.Models.Filtering.Operators
{
    /// <summary>
    /// Represents greater than filter operator.
    /// </summary>
    public class GreaterThanOperator : FilterOperator
    {
        /// <summary>
        /// Initializes new instance of <see cref="GreaterThanOperator"/> class.
        /// </summary>
        public GreaterThanOperator() : base(name: "greater than", @operator: ">")
        {

        }
    }
}
