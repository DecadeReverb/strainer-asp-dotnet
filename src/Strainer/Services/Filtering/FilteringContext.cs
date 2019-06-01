namespace Strainer.Services.Filtering
{
    public class FilteringContext : IFilteringContext
    {
        public FilteringContext(
            IFilterOperatorParser operatorParser,
            IFilterOperatorProvider operatorProvider,
            IFilterOperatorValidator operatorValidator,
            IFilterTermParser termParser)
        {
            OperatorParser = operatorParser;
            OperatorProvider = operatorProvider;
            OperatorValidator = operatorValidator;
            TermParser = termParser;
        }

        public IFilterOperatorParser OperatorParser { get; }

        public IFilterOperatorProvider OperatorProvider { get; }

        public IFilterOperatorValidator OperatorValidator { get; }

        public IFilterTermParser TermParser { get; }
    }
}
