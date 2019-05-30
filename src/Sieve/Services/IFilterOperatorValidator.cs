using System.Collections.Generic;
using Sieve.Models;

namespace Sieve.Services
{
    public interface IFilterOperatorValidator
    {
        void Validate(IFilterOperator filterOperator);
        void Validate(IEnumerable<IFilterOperator> filterOperators);
    }
}
