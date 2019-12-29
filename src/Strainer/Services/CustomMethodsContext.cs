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
        /// <param name="customFilterMethods">
        /// The custom filter methods dictionary.
        /// </param>
        /// <param name="customSortMethods">
        /// The custom sort methods dictionary.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="optionsProvider"/> is <see langword="null"/>.
        /// </exception>
        public CustomMethodsContext(
            ICustomFilterMethodDictionary customFilterMethods,
            ICustomSortMethodDictionary customSortMethods)
        {
            Filter = customFilterMethods ?? throw new ArgumentNullException(nameof(customFilterMethods));
            Sort = customSortMethods ?? throw new ArgumentNullException(nameof(customSortMethods));
        }

        /// <summary>
        /// Gets the custom filter method dictionary.
        /// </summary>
        public ICustomFilterMethodDictionary Filter { get; }

        /// <summary>
        /// Gets the custom sort method dictionary.
        /// </summary>
        public ICustomSortMethodDictionary Sort { get; }
    }
}
