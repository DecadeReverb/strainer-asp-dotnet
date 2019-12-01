using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;
using System;

namespace Fluorite.Strainer.Services
{
    /// <summary>
    /// Provides access for custom methods related services.
    /// </summary>
    public class CustomMethodsContext : ICustomMethodsContext
    {
        /// <summary>
        /// Initializes a new instance of <see cref="CustomMethodsContext"/> class.
        /// </summary>
        /// <param name="optionsProvider">
        /// The Strainer options provider.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="optionsProvider"/> is <see langword="null"/>.
        /// </exception>
        public CustomMethodsContext(IStrainerOptionsProvider optionsProvider)
        {
            if (optionsProvider == null)
            {
                throw new ArgumentNullException(nameof(optionsProvider));
            }

            Filter = new CustomFilterMethodMapper(optionsProvider);
            Sort = new CustomSortMethodMapper(optionsProvider);
        }

        /// <summary>
        /// Gets the custom filter method mapper.
        /// </summary>
        public ICustomFilterMethodMapper Filter { get; }

        /// <summary>
        /// Gets the custom sort method mapper.
        /// </summary>
        public ICustomSortMethodMapper Sort { get; }
    }
}
