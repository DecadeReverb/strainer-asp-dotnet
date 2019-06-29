using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.Services
{
    public interface IStrainerContext
    {
        ICustomMethodsContext CustomMethods { get; }
        IFilteringContext Filtering { get; }
        IStrainerPropertyMapper Mapper { get; }
        IStrainerPropertyMetadataProvider MetadataProvider { get; }
        StrainerOptions Options { get; }
        ISortingContext Sorting { get; }
    }
}
