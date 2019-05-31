namespace Sieve.Services.Filtering
{
    public class FilterTermContext : IFilterTermContext
    {
        public FilterTermContext(IFilterTermParser parser)
        {
            Parser = parser;
        }

        public IFilterTermParser Parser { get; }
    }
}
