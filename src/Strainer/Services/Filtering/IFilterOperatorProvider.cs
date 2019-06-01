using System.Collections.Generic;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterOperatorProvider
    {
        IReadOnlyList<IFilterOperator> Operators { get; }

        void AddOperator(IFilterOperator @operator);
        IFilterOperator GetDefaultOperator();
        IFilterOperator GetFirstOrDefault(string @operator);
    }
}
