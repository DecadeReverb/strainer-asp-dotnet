using System.Collections.Generic;
using Sieve.Models;

namespace Sieve.Services
{
    public interface IFilterTermOperator
    {
        IList<IFilterTerm> ParseFilterTerms();
    }
}
