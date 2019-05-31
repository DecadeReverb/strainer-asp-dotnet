using Sieve.Models;
using Sieve.Services.Filtering;
using Sieve.Services.Sorting;

namespace Sieve.Services
{
    public interface ISieveContext
    {
        ISieveCustomMethodsContext CustomMethodsContext { get; }
        IFilteringContext FilteringContext { get; }
        ISievePropertyMapper Mapper { get; }
        SieveOptions Options { get; }
        ISortingContext SortingContext { get; }
    }
}
