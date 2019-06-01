using System.Collections.Generic;
using Strainer.Models;

namespace Strainer.Services.Filtering
{
    public interface IFilterOperatorValidator
    {
        void Validate(IFilterOperator filterOperator);
        void Validate(IEnumerable<IFilterOperator> filterOperators);
    }
}
