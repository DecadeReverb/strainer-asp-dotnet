using System.Collections.Generic;
using Sieve.Models;

namespace Sieve.Services
{
    public interface IFilterOperatorProvider
    {
        void AddOperator(IFilterOperator @operator);
        IReadOnlyList<IFilterOperator> GetOperators();
        IFilterOperator GetFirstOrDefault(string @operator);
    }
}
