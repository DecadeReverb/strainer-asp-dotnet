using Sieve.Models;

namespace Sieve.Services
{
    public class SieveProcessor<TSortTerm>
        : SieveProcessor<SieveModel<TSortTerm>, TSortTerm>, ISieveProcessor<TSortTerm>
        where TSortTerm : ISortTerm, new()
    {
        public SieveProcessor(ISieveContext context) : base(context)
        {

        }
    }
}
