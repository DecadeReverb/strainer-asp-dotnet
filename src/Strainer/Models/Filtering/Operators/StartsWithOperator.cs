namespace Fluorite.Strainer.Models.Filtering.Operators
{
    /// <summary>
    /// Represents starts with filter operator.
    /// </summary>
    public class StartsWithOperator : FilterOperator
    {
        /// <summary>
        /// Initializes new instance of <see cref="StartsWithOperator"/> class.
        /// </summary>
        public StartsWithOperator() : base(name: "starts with", @operator: "_=")
        {

        }
    }
}
