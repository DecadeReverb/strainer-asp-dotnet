using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;
using System;

namespace Fluorite.Strainer.Services
{
    public class CustomMethodsContext : ICustomMethodsContext
    {
        public CustomMethodsContext(IStrainerOptionsProvider optionsProvider)
        {
            if (optionsProvider == null)
            {
                throw new ArgumentNullException(nameof(optionsProvider));
            }

            Filter = new CustomFilterMethodMapper(optionsProvider);
            Sort = new CustomSortMethodMapper(optionsProvider);
        }

        public ICustomFilterMethodMapper Filter { get; }

        public ICustomSortMethodMapper Sort { get; }
    }
}
