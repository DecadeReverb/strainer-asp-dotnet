using System;

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
            ExpressionProvider = expressionProvider ?? throw new ArgumentNullException(nameof(expressionProvider));
            OperatorMapper = operatorMapper ?? throw new ArgumentNullException(nameof(operatorMapper));
            OperatorParser = operatorParser ?? throw new ArgumentNullException(nameof(operatorParser));
            OperatorValidator = operatorValidator ?? throw new ArgumentNullException(nameof(operatorValidator));
            TermParser = termParser ?? throw new ArgumentNullException(nameof(termParser));
        }

        public IFilterExpressionProvider ExpressionProvider { get; }

        public IFilterOperatorMapper OperatorMapper { get; }

        public IFilterOperatorParser OperatorParser { get; }

        public IFilterOperatorValidator OperatorValidator { get; }

        public IFilterTermParser TermParser { get; }
    }
}
