using Fluorite.Strainer.Models.Filtering.Terms;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterTermSectionsParser
    {
        FilterTermSections Parse(string input);
    }
}
