using Fluorite.Strainer.Models.Filtering.Terms;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterTermParser
    {
        IList<IFilterTerm> GetParsedTerms(string input);
    }
}
