namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterContext : IFilterContext
    {
        public FilterContext(
            IFilterExpressionProvider expressionProvider,
            IFilterOperatorMapper operatorMapper,
            IFilterOperatorParser operatorParser,
            IFilterOperatorValidator operatorValidator,
            IFilterTermParser termParser)
        {
            ExpressionProvider = expressionProvider ?? throw new System.ArgumentNullException(nameof(expressionProvider));
            OperatorMapper = operatorMapper ?? throw new System.ArgumentNullException(nameof(operatorMapper));
            OperatorParser = operatorParser ?? throw new System.ArgumentNullException(nameof(operatorParser));
            OperatorValidator = operatorValidator ?? throw new System.ArgumentNullException(nameof(operatorValidator));
            TermParser = termParser ?? throw new System.ArgumentNullException(nameof(termParser));
        }

        public IFilterExpressionProvider ExpressionProvider { get; }

        public IFilterOperatorMapper OperatorMapper { get; }

        public IFilterOperatorParser OperatorParser { get; }

        public IFilterOperatorValidator OperatorValidator { get; }

        public IFilterTermParser TermParser { get; }
    }
}
