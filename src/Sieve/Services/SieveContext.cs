using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services.Filtering;
using Sieve.Services.Sorting;

namespace Sieve.Services
{
    public class SieveContext : ISieveContext
    {
        public SieveContext(
            IOptions<SieveOptions> options,
            IFilteringContext filteringContext,
            ISortingContext sortingContext,
            ISievePropertyMapper mapper,
            ISieveCustomMethodsContext customMethodsContext)
        {
            CustomMethodsContext = customMethodsContext;
            FilteringContext = filteringContext;
            SortingContext = sortingContext;
            Mapper = mapper;
            Options = options.Value;
        }

        public ISieveCustomMethodsContext CustomMethodsContext { get; }

        public IFilteringContext FilteringContext { get; }

        public ISievePropertyMapper Mapper { get; }

        public SieveOptions Options { get; }

        public ISortingContext SortingContext { get; }
    }
}
