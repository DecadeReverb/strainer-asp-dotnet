using Fluorite.Strainer.Models.Filtering.Operators;
using System.Collections.Generic;

namespace Fluorite.Strainer.Models.Filtering.Terms
{
    public interface IFilterTerm
    {
        string Input { get; }
        IList<string> Names { get; }
        string Operator { get; }
        bool OperatorIsCaseInsensitive { get; }
        bool OperatorIsNegated { get; }
        IFilterOperator OperatorParsed { get; }
        IList<string> Values { get; }
    }
}
