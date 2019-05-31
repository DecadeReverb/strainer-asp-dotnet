using Sieve.Models;

namespace Sieve.Services.Filtering
{
    public interface IFilterOperatorParser
    {
        IFilterOperator GetParsedOperator(string input);
        IFilterOperator GetParsedOperatorAsUnnegated(string input);
    }
}
