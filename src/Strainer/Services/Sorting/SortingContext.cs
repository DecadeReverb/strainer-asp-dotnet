namespace Fluorite.Strainer.Services.Sorting
{
    public class SortingContext : ISortingContext
    {
        public SortingContext(ISortingWayFormatter formatter, ISortTermParser parser)
        {
            Formatter = formatter;
            TermParser = parser;
        }

        public ISortingWayFormatter Formatter { get; }

        public ISortTermParser TermParser { get; }
    }
}
