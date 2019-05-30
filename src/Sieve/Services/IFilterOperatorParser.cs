using Sieve.Models;

namespace Sieve.Services
{
    public interface IFilterOperatorParser
    {
        IFilterOperator GetParsedOperator(string input);
        IFilterOperator GetParsedOperatorAsUnnegated(string input);
    }
}
