using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.Services
{
    public interface IStrainerCustomMethodsContext
    {
        IStrainerCustomFilterMethods Filter { get; }
        ICustomSortMethodProvider Sort { get; }
    }
}
