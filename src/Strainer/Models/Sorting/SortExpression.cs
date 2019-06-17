namespace Fluorite.Strainer.Models.Sorting
{
    /// <summary>
    /// Provides additional information about expression used for sorting.
    /// </summary>
    public class SortExpression : ISortExpression
    {
        /// <summary>
        /// Initializes new instance of <see cref="SortExpression"/> class.
        /// </summary>
        public SortExpression()
        {

        }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indictating whether related
        /// expression should be used for ordering in descending way.
        /// </summary>
        public bool IsDescending { get; set; }

        /// <summary>
        /// Gets or sets a <see cref="bool"/> value indictating whether related
        /// expression should be used for subsequent ordering e.g. using
        /// ThenBy() or ThenByDescending().
        /// </summary>
        public bool IsSubsequent { get; set; }
    }
}
