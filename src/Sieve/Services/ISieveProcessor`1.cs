using Sieve.Models;

namespace Sieve.Services
{
    public interface ISieveProcessor<TSortTerm>
        : ISieveProcessor<SieveModel<TSortTerm>, TSortTerm>
        where TSortTerm : ISortTerm, new()
    {

    }
}
