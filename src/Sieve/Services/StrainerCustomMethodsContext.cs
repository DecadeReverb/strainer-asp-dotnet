namespace Strainer.Services
{
    public class StrainerCustomMethodsContext : IStrainerCustomMethodsContext
    {
        public StrainerCustomMethodsContext()
        {

        }

        public StrainerCustomMethodsContext(IStrainerCustomFilterMethods filterMethods)
        {
            FilterMethods = filterMethods;
        }

        public StrainerCustomMethodsContext(IStrainerCustomSortMethods sortMethods)
        {
            SortMethods = sortMethods;
        }

        public StrainerCustomMethodsContext(IStrainerCustomFilterMethods filterMethods, IStrainerCustomSortMethods sortMethods)
        {
            FilterMethods = filterMethods;
            SortMethods = sortMethods;
        }

        public IStrainerCustomFilterMethods FilterMethods { get; }

        public IStrainerCustomSortMethods SortMethods { get; }
    }
}
