using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.Services
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
