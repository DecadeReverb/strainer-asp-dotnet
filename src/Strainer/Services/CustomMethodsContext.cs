using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Filtering;
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

            var sortMapper = new CustomSortMethodMapper(options);
            Sort = new CustomSortMethodProvider(sortMapper);
        }

        public CustomMethodsContext(StrainerOptions options, ICustomFilterMethodProvider filterMethods)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Filter = filterMethods ?? throw new ArgumentNullException(nameof(filterMethods));

            var mapper = new CustomSortMethodMapper(options);
            Sort = new CustomSortMethodProvider(mapper);
        }

        public CustomMethodsContext(StrainerOptions options, ICustomSortMethodProvider sortMethods)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Sort = sortMethods ?? throw new ArgumentNullException(nameof(sortMethods));

            var mapper = new CustomFilterMethodMapper(options);
            Filter = new CustomFilterMethodProvider(mapper);
        }

        public CustomMethodsContext(StrainerOptions options, ICustomFilterMethodProvider filterMethods, ICustomSortMethodProvider sortMethods)
        {
            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            Filter = filterMethods ?? throw new ArgumentNullException(nameof(filterMethods));
            Sort = sortMethods ?? throw new ArgumentNullException(nameof(sortMethods));
        }

        public ICustomFilterMethodProvider Filter { get; }

        public ICustomSortMethodProvider Sort { get; }
    }
}
