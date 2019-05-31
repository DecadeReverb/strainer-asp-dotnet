using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services.Filtering;

namespace Sieve.Services
{
    public class SieveContext : ISieveContext
    {
        public SieveContext(
            IOptions<SieveOptions> options,
            IFilterOperatorContext filterOperatorContext,
            IFilterTermContext filterTermContext,
            ISievePropertyMapper mapper,
            ISieveCustomMethodsContext customMethodsContext)
        {
            CustomMethodsContext = customMethodsContext;
            FilterOperatorContext = filterOperatorContext;
            FilterTermContext = filterTermContext;
            Mapper = mapper;
            Options = options.Value;
        }

        public ISieveCustomMethodsContext CustomMethodsContext { get; }

        public IFilterOperatorContext FilterOperatorContext { get; }

        public IFilterTermContext FilterTermContext { get; }

        public ISievePropertyMapper Mapper { get; }

        public SieveOptions Options { get; }
    }
}
