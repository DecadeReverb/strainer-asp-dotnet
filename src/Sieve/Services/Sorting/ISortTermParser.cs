using System.Collections.Generic;
using Strainer.Models;

namespace Strainer.Services.Sorting
{
    public interface ISortTermParser
    {
        IList<ISortTerm> GetParsedTerms(string input);
    }
}
