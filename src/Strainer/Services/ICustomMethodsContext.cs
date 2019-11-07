using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.Services
{
    public interface ICustomMethodsContext
    {
        ICustomFilterMethodMapper Filter { get; }
        ICustomSortMethodMapper Sort { get; }
    }
}
