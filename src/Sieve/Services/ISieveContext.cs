using Sieve.Models;
using Sieve.Services.Filtering;

namespace Sieve.Services
{
    public interface ISieveContext
    {
        ISieveCustomMethodsContext CustomMethodsContext { get; }
        IFilterOperatorContext FilterOperatorContext { get; }
        IFilterTermContext FilterTermContext { get; }
        ISievePropertyMapper Mapper { get; }
        SieveOptions Options { get; }
    }
}
