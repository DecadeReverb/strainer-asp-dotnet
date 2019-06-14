namespace Fluorite.Strainer.Services.Filtering
{
    public class FilteringContext : IFilteringContext
    {
        public FilteringContext(
            IFilterOperatorMapper operatorMapper,
            IFilterOperatorParser operatorParser,
            IFilterOperatorValidator operatorValidator,
            IFilterTermParser termParser)
        {
            OperatorMapper = operatorMapper;
            OperatorParser = operatorParser;
            OperatorValidator = operatorValidator;
            TermParser = termParser;
        }

        public IFilterOperatorMapper OperatorMapper { get; }

        public IFilterOperatorParser OperatorParser { get; }

        public IFilterOperatorValidator OperatorValidator { get; }

        public IFilterTermParser TermParser { get; }
    }
}
