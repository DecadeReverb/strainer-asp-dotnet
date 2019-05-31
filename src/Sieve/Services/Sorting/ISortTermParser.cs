using System.Collections.Generic;
using Sieve.Models;

namespace Sieve.Services.Sorting
{
    public interface ISortTermParser
    {
        IList<ISortTerm> GetParsedTerms(string input);
    }
}
