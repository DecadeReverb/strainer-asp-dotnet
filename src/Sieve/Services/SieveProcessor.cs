using Sieve.Models;

namespace Sieve.Services
{
    public class SieveProcessor : SieveProcessor<SieveModel, FilterTerm, SortTerm>, ISieveProcessor
    {
        public SieveProcessor(ISieveContext context) : base(context)
        {

        }
    }
}
