using Fluorite.Strainer.Models.Filtering.Operators;
using System;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterContext : IFilterContext
    {
        public FilterContext(
            IFilterExpressionProvider expressionProvider,
            IReadOnlyDictionary<string, IFilterOperator> filterOperatorsDictionary,
            IFilterOperatorParser operatorParser,
            IFilterOperatorValidator operatorValidator,
            IFilterTermParser termParser)
        {
            ExpressionProvider = expressionProvider ?? throw new ArgumentNullException(nameof(expressionProvider));
            OperatorDictionary = filterOperatorsDictionary ?? throw new ArgumentNullException(nameof(filterOperatorsDictionary));
            OperatorParser = operatorParser ?? throw new ArgumentNullException(nameof(operatorParser));
            OperatorValidator = operatorValidator ?? throw new ArgumentNullException(nameof(operatorValidator));
            TermParser = termParser ?? throw new ArgumentNullException(nameof(termParser));
        }

        public IFilterExpressionProvider ExpressionProvider { get; }

        public IReadOnlyDictionary<string, IFilterOperator> OperatorDictionary { get; }

        public IFilterOperatorParser OperatorParser { get; }

        public IFilterOperatorValidator OperatorValidator { get; }

        public IFilterTermParser TermParser { get; }
    }
}
