using Microsoft.Extensions.Options;
using Strainer.Models;
using Strainer.Services.Filtering;
using Strainer.Services.Sorting;

namespace Strainer.Services
{
    public class StrainerContext : IStrainerContext
    {
        public StrainerContext(
            IOptions<StrainerOptions> options,
            IFilteringContext filteringContext,
            ISortingContext sortingContext,
            IStrainerPropertyMapper mapper,
            IStrainerCustomMethodsContext customMethodsContext)
        {
            CustomMethodsContext = customMethodsContext;
            FilteringContext = filteringContext;
            SortingContext = sortingContext;
            Mapper = mapper;
            Options = options.Value;
        }

        public IStrainerCustomMethodsContext CustomMethodsContext { get; }

        public IFilteringContext FilteringContext { get; }

        public IStrainerPropertyMapper Mapper { get; }

        public StrainerOptions Options { get; }

        public ISortingContext SortingContext { get; }
    }
}
