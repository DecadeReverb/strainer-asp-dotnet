using System.Collections.Generic;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Sorting
{
    public interface ISortTermParser
    {
        IList<ISortTerm> GetParsedTerms(string input);
    }
}
