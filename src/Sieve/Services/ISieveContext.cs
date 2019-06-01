using Strainer.Models;
using Strainer.Services.Filtering;
using Strainer.Services.Sorting;

namespace Strainer.Services
{
    public interface IStrainerContext
    {
        IStrainerCustomMethodsContext CustomMethodsContext { get; }
        IFilteringContext FilteringContext { get; }
        IStrainerPropertyMapper Mapper { get; }
        StrainerOptions Options { get; }
        ISortingContext SortingContext { get; }
    }
}
