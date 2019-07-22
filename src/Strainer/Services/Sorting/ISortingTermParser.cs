using Fluorite.Strainer.Models.Sorting.Terms;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Sorting
{
    public interface ISortingTermParser
    {
        IList<ISortTerm> GetParsedTerms(string input);
    }
}
