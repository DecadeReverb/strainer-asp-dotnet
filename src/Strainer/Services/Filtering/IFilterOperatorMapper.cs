using Fluorite.Strainer.Models.Filtering.Operators;
using System.Collections;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterOperatorMapper :
        IDictionary<string, IFilterOperator>,
        ICollection<KeyValuePair<string, IFilterOperator>>,
        IEnumerable<KeyValuePair<string, IFilterOperator>>,
        IEnumerable
    {
        IFilterOperatorBuilder Operator(string symbol);
    }
}
