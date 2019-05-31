namespace Sieve.Services.Filtering
{
    public class FilterOperatorContext : IFilterOperatorContext
    {
        public FilterOperatorContext(IFilterOperatorParser parser, IFilterOperatorProvider provider, IFilterOperatorValidator validator)
        {
            Parser = parser;
            Provider = provider;
            Validator = validator;
        }

        public IFilterOperatorParser Parser { get; }

        public IFilterOperatorProvider Provider { get; }

        public IFilterOperatorValidator Validator { get; }
    }
}
