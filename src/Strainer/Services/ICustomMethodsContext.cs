using Fluorite.Strainer.Services.Filter;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.Services
{
    public interface ICustomMethodsContext
    {
        ICustomFilterMethodProvider Filter { get; }
        ICustomSortingMethodProvider Sorting { get; }
    }
}
