using Sieve.Models;

namespace Sieve.Services
{
    public class SieveProcessor<TFilterTerm, TSortTerm>
        : SieveProcessor<SieveModel<TFilterTerm, TSortTerm>, TFilterTerm, TSortTerm>, ISieveProcessor<TFilterTerm, TSortTerm>
        where TFilterTerm : IFilterTerm, new()
        where TSortTerm : ISortTerm, new()
    {
        public SieveProcessor(ISieveContext context) : base(context)
        {

        }
    }
}
