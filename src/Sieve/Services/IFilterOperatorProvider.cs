using System.Collections.Generic;
using Sieve.Models;

namespace Sieve.Services
{
    public interface IFilterOperatorProvider
    {
        IReadOnlyList<IFilterOperator> GetOperators();
    }
}
