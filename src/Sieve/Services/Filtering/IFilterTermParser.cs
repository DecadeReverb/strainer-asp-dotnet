using System.Collections.Generic;
using Strainer.Models;

namespace Strainer.Services.Filtering
{
    public interface IFilterTermParser
    {
        IList<IFilterTerm> GetParsedTerms(string input);
    }
}
