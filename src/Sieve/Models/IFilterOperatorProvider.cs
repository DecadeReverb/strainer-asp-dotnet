using System.Collections.Generic;

namespace Sieve.Models
{
    public interface IFilterOperatorProvider
    {
        IReadOnlyList<IFilterOperator> GetOperators();
    }
}
