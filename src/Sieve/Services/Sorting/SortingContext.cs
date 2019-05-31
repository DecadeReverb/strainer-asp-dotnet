namespace Sieve.Services.Sorting
{
    public class SortingContext : ISortingContext
    {
        public SortingContext(ISortTermParser parser)
        {
            TermParser = parser;
        }

        public ISortTermParser TermParser { get; }
    }
}
