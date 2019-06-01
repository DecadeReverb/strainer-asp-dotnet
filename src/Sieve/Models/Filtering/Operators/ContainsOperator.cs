namespace Strainer.Models.Filtering.Operators
{
    /// <summary>
    /// Represents contains filter operator.
    /// </summary>
    public class ContainsOperator : FilterOperator
    {
        /// <summary>
        /// Initializes new instance of <see cref="ContainsOperator"/> class.
        /// </summary>
        public ContainsOperator() : base(name: "contains", @operator: "@=")
        {

        }
    }
}
