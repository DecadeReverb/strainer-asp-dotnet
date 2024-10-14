using Fluorite.Strainer.Models.Sorting.Terms;

namespace Fluorite.Strainer.Services.Sorting;

public interface ISortTermParser
{
    IList<ISortTerm> GetParsedTerms(string input);
}
