using Fluorite.Strainer.Models.Filtering.Operators;
using System.Collections;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterOperatorDictionary :
        IReadOnlyDictionary<string, IFilterOperator>,
        IReadOnlyCollection<KeyValuePair<string, IFilterOperator>>,
        IEnumerable<KeyValuePair<string, IFilterOperator>>,
        IEnumerable
    {

    }
}