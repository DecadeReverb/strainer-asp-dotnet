using System.Collections.Generic;
using Strainer.Models;

namespace Strainer.Services.Filtering
{
    public interface IFilterTermOperator
    {
        IList<IFilterTerm> ParseFilterTerms();
    }
}
