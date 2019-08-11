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

            Filter = new CustomFilterMethodMapper(options);
            Sort = new CustomSortMethodMapper(options);
        }

        public ICustomFilterMethodMapper Filter { get; }

        public ICustomSortMethodMapper Sort { get; }
    }
}
