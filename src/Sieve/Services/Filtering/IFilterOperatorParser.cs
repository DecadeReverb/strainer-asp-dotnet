using Strainer.Models;

namespace Strainer.Services.Filtering
{
    public interface IFilterOperatorParser
    {
        IFilterOperator GetParsedOperator(string input);
        IFilterOperator GetParsedOperatorAsUnnegated(string input);
    }
}
