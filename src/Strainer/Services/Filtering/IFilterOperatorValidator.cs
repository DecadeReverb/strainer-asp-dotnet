using System.Collections.Generic;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterOperatorValidator
    {
        void Validate(IFilterOperator filterOperator);
        void Validate(IEnumerable<IFilterOperator> filterOperators);
    }
}
