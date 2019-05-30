using System.Collections.Generic;

namespace Sieve.Models
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
