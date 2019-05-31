using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services.Filtering;

namespace Sieve.Services
{
    public class SieveProcessor<TFilterTerm, TSortTerm>
        : SieveProcessor<SieveModel<TFilterTerm, TSortTerm>, TFilterTerm, TSortTerm>, ISieveProcessor<TFilterTerm, TSortTerm>
        where TFilterTerm : IFilterTerm, new()
        where TSortTerm : ISortTerm, new()
    {
        public SieveProcessor(
            IOptions<SieveOptions> options,
            IFilterOperatorProvider filterOperatorProvider,
            IFilterTermParser filterTermParser)
            : base(options, filterOperatorProvider, filterTermParser)
        {

        }

        public SieveProcessor(
            IOptions<SieveOptions> options,
            IFilterOperatorProvider filterOperatorProvider,
            IFilterTermParser filterTermParser,
            ISieveCustomSortMethods customSortMethods)
            : base(options, filterOperatorProvider, filterTermParser, customSortMethods)
        {

        }

        public SieveProcessor(
            IOptions<SieveOptions> options,
            IFilterOperatorProvider filterOperatorProvider,
            IFilterTermParser filterTermParser,
            ISieveCustomFilterMethods customFilterMethods)
            : base(options, filterOperatorProvider, filterTermParser, customFilterMethods)
        {

        }

        public SieveProcessor(
            IOptions<SieveOptions> options,
            IFilterOperatorProvider filterOperatorProvider,
            IFilterTermParser filterTermParser,
            ISieveCustomSortMethods customSortMethods,
            ISieveCustomFilterMethods customFilterMethods)
            : base(options, filterOperatorProvider, filterTermParser, customSortMethods, customFilterMethods)
        {

        }
    }
}
