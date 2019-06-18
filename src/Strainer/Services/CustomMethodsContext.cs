using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.Services
{
    public class CustomMethodsContext : ICustomMethodsContext
    {
        public CustomMethodsContext()
        {

        }

        public CustomMethodsContext(ICustomFilterMethodProvider filterMethods)
        {
            Filter = filterMethods;
        }

        public CustomMethodsContext(ICustomSortMethodProvider sortMethods)
        {
            Sort = sortMethods;
        }

        public CustomMethodsContext(ICustomFilterMethodProvider filterMethods, ICustomSortMethodProvider sortMethods)
        {
            Filter = filterMethods;
            Sort = sortMethods;
        }

        public ICustomFilterMethodProvider Filter { get; }

        public ICustomSortMethodProvider Sort { get; }
    }
}
