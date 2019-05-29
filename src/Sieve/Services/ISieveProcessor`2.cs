using Sieve.Models;

namespace Sieve.Services
{
    public interface ISieveProcessor<TFilterTerm, TSortTerm>
        : ISieveProcessor<SieveModel<TFilterTerm, TSortTerm>, TFilterTerm, TSortTerm>
        where TFilterTerm : IFilterTerm, new()
        where TSortTerm : ISortTerm, new()
    {

    }
}
