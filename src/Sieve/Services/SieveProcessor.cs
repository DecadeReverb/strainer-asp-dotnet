using Sieve.Models;

namespace Sieve.Services
{
    public class SieveProcessor : SieveProcessor<SieveModel>, ISieveProcessor
    {
        public SieveProcessor(ISieveContext context) : base(context)
        {

        }
    }
}
