using Sieve.Models;

namespace Sieve.Services
{
    public interface IFilterOperatorBuilder
    {
        IFilterOperator Build();
        IFilterOperatorBuilder CaseInsensitive();
        IFilterOperatorBuilder HasName(string name);
        IFilterOperatorBuilder HasUnnegatedOperator(IFilterOperator filterOperator);
        IFilterOperatorBuilder Negated();
        IFilterOperatorBuilder Operator();
    }
}
