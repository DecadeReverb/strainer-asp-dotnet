using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.Services
{
    public interface ICustomMethodsContext
    {
        ICustomFilterMethodProvider Filter { get; }
        ICustomSortMethodProvider Sort { get; }
    }
}
