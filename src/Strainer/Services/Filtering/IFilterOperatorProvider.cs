using Fluorite.Strainer.Models.Filtering.Operators;
using System.Collections;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterOperatorProvider : IEnumerable<IFilterOperator>, IEnumerable
    {
        IFilterOperator GetDefaultOperator();
    }
}
