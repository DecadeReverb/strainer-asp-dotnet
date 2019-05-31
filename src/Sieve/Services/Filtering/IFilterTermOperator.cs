using System.Collections.Generic;
using Sieve.Models;

namespace Sieve.Services.Filtering
{
    public interface IFilterTermOperator
    {
        IList<IFilterTerm> ParseFilterTerms();
    }
}
