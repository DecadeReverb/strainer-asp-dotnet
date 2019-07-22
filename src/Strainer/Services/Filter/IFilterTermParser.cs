using Fluorite.Strainer.Models.Filtering.Terms;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Filter
{
    public interface IFilterTermParser
    {
        IList<IFilterTerm> GetParsedTerms(string input);
    }
}
