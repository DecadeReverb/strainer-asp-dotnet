using System.Collections.Generic;
using Strainer.Models;

namespace Strainer.Services.Filtering
{
    public interface IFilterOperatorProvider
    {
        IReadOnlyList<IFilterOperator> Operators { get; }

        void AddOperator(IFilterOperator @operator);
        IFilterOperator GetDefaultOperator();
        IFilterOperator GetFirstOrDefault(string @operator);
    }
}
