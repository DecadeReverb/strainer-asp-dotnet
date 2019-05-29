using Microsoft.Extensions.Options;
using Sieve.Models;

namespace Sieve.Services
{
    public class SieveProcessor : SieveProcessor<SieveModel, FilterTerm, SortTerm>, ISieveProcessor
    {
        public SieveProcessor(IOptions<SieveOptions> options, IFilterOperatorProvider filterOperatorProvider) : base(options, filterOperatorProvider)
        {

        }

        public SieveProcessor(IOptions<SieveOptions> options, IFilterOperatorProvider filterOperatorProvider, ISieveCustomSortMethods customSortMethods)
            : base(options, filterOperatorProvider, customSortMethods)
        {

        }

        public SieveProcessor(IOptions<SieveOptions> options, IFilterOperatorProvider filterOperatorProvider, ISieveCustomFilterMethods customFilterMethods)
            : base(options, filterOperatorProvider, customFilterMethods)
        {

        }

        public SieveProcessor(IOptions<SieveOptions> options, IFilterOperatorProvider filterOperatorProvider, ISieveCustomSortMethods customSortMethods, ISieveCustomFilterMethods customFilterMethods)
            : base(options, filterOperatorProvider, customSortMethods, customFilterMethods)
        {

        }
    }
}
