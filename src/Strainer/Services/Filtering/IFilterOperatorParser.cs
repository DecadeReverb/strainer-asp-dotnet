using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services.Filtering
{
    public interface IFilterOperatorParser
    {
        IFilterOperator GetParsedOperator(string input);
        IFilterOperator GetParsedOperatorAsUnnegated(string input);
    }
}
