namespace Fluorite.Strainer.Services.Filtering
{
    public class FilteringContext : IFilteringContext
    {
        public FilteringContext(
            IFilterExpressionProvider expressionProvider,
            IFilterOperatorMapper operatorMapper,
            IFilterOperatorParser operatorParser,
            IFilterOperatorValidator operatorValidator,
            IFilterTermParser termParser)
        {
            ExpressionProvider = expressionProvider;
            OperatorMapper = operatorMapper;
            OperatorParser = operatorParser;
            OperatorValidator = operatorValidator;
            TermParser = termParser;
        }

        public IFilterExpressionProvider ExpressionProvider { get; }

        public IFilterOperatorMapper OperatorMapper { get; }

        public IFilterOperatorParser OperatorParser { get; }

        public IFilterOperatorValidator OperatorValidator { get; }

        public IFilterTermParser TermParser { get; }
    }
}
