using Sieve.Models;

namespace Sieve.Services
{
    public interface IFilterOperatorBuilder
    {
        IFilterOperator Build();
        IFilterOperatorBuilder CaseInsensitive();
        IFilterOperatorBuilder Default();
        IFilterOperatorBuilder HasCaseSensitiveVersion(IFilterOperator filterOperator);
        IFilterOperatorBuilder HasName(string name);
        IFilterOperatorBuilder HasUnnegatedVersion(IFilterOperator filterOperator);
        IFilterOperatorBuilder Operator(string @operator);
    }
}
