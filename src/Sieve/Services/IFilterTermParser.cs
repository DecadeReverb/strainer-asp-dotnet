using System.Collections.Generic;
using Sieve.Models;

namespace Sieve.Services
{
    public interface IFilterTermParser
    {
        IList<IFilterTerm> GetParsedFilterTerms(string input);
    }
}
