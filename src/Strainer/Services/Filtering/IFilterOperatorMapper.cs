using Fluorite.Strainer.Models.Filter.Operators;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterOperatorMapper
    {
        IReadOnlyCollection<IFilterOperator> Operators { get; }
        IReadOnlyCollection<string> Symbols { get; }

        void AddMap(string symbol, IFilterOperator filterOperator);
        IFilterOperator Find(string symbol);
        IFilterOperator GetDefault();
        IFilterOperatorBuilder Operator(string symbol);
    }
}
