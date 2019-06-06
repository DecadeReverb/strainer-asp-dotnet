using Fluorite.Strainer.Models.Filtering.Operators;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterOperatorValidator
    {
        void Validate(IFilterOperator filterOperator);
        void Validate(IEnumerable<IFilterOperator> filterOperators);
    }
}
