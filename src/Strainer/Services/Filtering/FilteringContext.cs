namespace Fluorite.Strainer.Services.Filtering
{
    public class FilteringContext : IFilteringContext
    {
        public FilteringContext(
            IFilterExpressionMapper expressionMapper,
            IFilterOperatorParser operatorParser,
            IFilterOperatorProvider operatorProvider,
            IFilterOperatorValidator operatorValidator,
            IFilterTermParser termParser)
        {
            ExpressionMapper = expressionMapper;
            OperatorParser = operatorParser;
            OperatorProvider = operatorProvider;
            OperatorValidator = operatorValidator;
            TermParser = termParser;
        }

        public IFilterExpressionMapper ExpressionMapper { get; }

        public IFilterOperatorParser OperatorParser { get; }

        public IFilterOperatorProvider OperatorProvider { get; }

        public IFilterOperatorValidator OperatorValidator { get; }

        public IFilterTermParser TermParser { get; }
    }
}
