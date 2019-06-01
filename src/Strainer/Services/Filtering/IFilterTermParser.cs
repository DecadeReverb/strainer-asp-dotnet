using System.Collections.Generic;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterTermParser
    {
        IList<IFilterTerm> GetParsedTerms(string input);
    }
}
