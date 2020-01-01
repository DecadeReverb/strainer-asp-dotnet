using Fluorite.Strainer.Models.Filtering.Operators;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterContext
    {
        IFilterExpressionProvider ExpressionProvider { get; }

        IReadOnlyDictionary<string, IFilterOperator> OperatorDictionary { get; }

        IFilterOperatorParser OperatorParser { get; }

        IFilterOperatorValidator OperatorValidator { get; }

        IFilterTermParser TermParser { get; }
    }
}
