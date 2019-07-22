using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Filter;
using Fluorite.Strainer.Services.Sorting;
using System;

namespace Fluorite.Strainer.Services
{
    public class CustomMethodsContext : ICustomMethodsContext
    {
        public CustomMethodsContext(StrainerOptions options)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var filterMapper = new CustomFilterMethodMapper(options);
            Filter = new CustomFilterMethodProvider(filterMapper);

            var sortMapper = new CustomSortingMethodMapper(options);
            Sorting = new CustomSortingMethodProvider(sortMapper);
        }

        public CustomMethodsContext(StrainerOptions options, ICustomFilterMethodProvider filterMethods)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Filter = filterMethods ?? throw new ArgumentNullException(nameof(filterMethods));

            var mapper = new CustomSortingMethodMapper(options);
            Sorting = new CustomSortingMethodProvider(mapper);
        }

        public CustomMethodsContext(StrainerOptions options, ICustomSortingMethodProvider sortMethods)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Sorting = sortMethods ?? throw new ArgumentNullException(nameof(sortMethods));

            var mapper = new CustomFilterMethodMapper(options);
            Filter = new CustomFilterMethodProvider(mapper);
        }

        public CustomMethodsContext(StrainerOptions options, ICustomFilterMethodProvider filterMethods, ICustomSortingMethodProvider sortMethods)
        {
            Filter = filterMethods ?? throw new ArgumentNullException(nameof(filterMethods));
            Sorting = sortMethods ?? throw new ArgumentNullException(nameof(sortMethods));
        }

        public ICustomFilterMethodProvider Filter { get; }

        public ICustomSortingMethodProvider Sorting { get; }
    }
}
