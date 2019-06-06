using System.Collections;
using System.Collections.Generic;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterOperatorProvider : IEnumerable<IFilterOperator>, IEnumerable
    {
        IFilterOperator GetDefaultOperator();
    }
}
