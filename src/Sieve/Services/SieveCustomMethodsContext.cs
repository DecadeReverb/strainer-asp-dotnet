namespace Sieve.Services
{
    public class SieveCustomMethodsContext : ISieveCustomMethodsContext
    {
        public SieveCustomMethodsContext()
        {

        }

        public SieveCustomMethodsContext(ISieveCustomFilterMethods filterMethods)
        {
            FilterMethods = filterMethods;
        }

        public SieveCustomMethodsContext(ISieveCustomSortMethods sortMethods)
        {
            SortMethods = sortMethods;
        }

        public SieveCustomMethodsContext(ISieveCustomFilterMethods filterMethods, ISieveCustomSortMethods sortMethods)
        {
            FilterMethods = filterMethods;
            SortMethods = sortMethods;
        }

        public ISieveCustomFilterMethods FilterMethods { get; }

        public ISieveCustomSortMethods SortMethods { get; }
    }
}
