namespace Fluorite.Strainer.Models.Filtering.Terms
{
    /// <summary>
    /// Represents different sections of partially parsed filter term.
    /// </summary>
    public class FilterTermSections
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FilterTermSections"/> class.
        /// </summary>
        public FilterTermSections()
        {
        }

        public string Names { get; set; }

        public string OperatorSymbol { get; set; }

        public string Values { get; set; }
    }
}
