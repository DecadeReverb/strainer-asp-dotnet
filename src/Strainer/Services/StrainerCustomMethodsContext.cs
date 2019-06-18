using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.Services
{
    public class StrainerCustomMethodsContext : IStrainerCustomMethodsContext
    {
        public StrainerCustomMethodsContext()
        {

        }

        public StrainerCustomMethodsContext(IStrainerCustomFilterMethods filterMethods)
        {
            Filter = filterMethods;
        }

        public StrainerCustomMethodsContext(ICustomSortMethodProvider sortMethods)
        {
            Sort = sortMethods;
        }

        public StrainerCustomMethodsContext(IStrainerCustomFilterMethods filterMethods, ICustomSortMethodProvider sortMethods)
        {
            Filter = filterMethods;
            Sort = sortMethods;
        }

        public IStrainerCustomFilterMethods Filter { get; }

        public ICustomSortMethodProvider Sort { get; }
    }
}
