using System;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterContext : IFilterContext
    {
        public FilterContext(
            IFilterExpressionProvider expressionProvider,
            IFilterOperatorParser operatorParser,
            IFilterOperatorValidator operatorValidator,
            IFilterTermParser termParser)
        {
            ExpressionProvider = expressionProvider ?? throw new ArgumentNullException(nameof(expressionProvider));
            OperatorParser = operatorParser ?? throw new ArgumentNullException(nameof(operatorParser));
            OperatorValidator = operatorValidator ?? throw new ArgumentNullException(nameof(operatorValidator));
            TermParser = termParser ?? throw new ArgumentNullException(nameof(termParser));
        }

        public IFilterExpressionProvider ExpressionProvider { get; }

        public IFilterOperatorParser OperatorParser { get; }

        public IFilterOperatorValidator OperatorValidator { get; }

        public IFilterTermParser TermParser { get; }
    }
}
