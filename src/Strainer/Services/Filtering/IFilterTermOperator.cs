using System.Collections.Generic;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterTermOperator
    {
        IList<IFilterTerm> ParseFilterTerms();
    }
}
