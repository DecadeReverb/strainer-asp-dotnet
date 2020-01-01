using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Sorting;
using System;
using System.Collections.Generic;

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
        /// <paramref name="customFilterMethods"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="customSortMethods"/> is <see langword="null"/>.
        /// </exception>
        public CustomMethodsContext(
            IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>> customFilterMethods,
            IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>> customSortMethods)
        {
            Filter = customFilterMethods ?? throw new ArgumentNullException(nameof(customFilterMethods));
            Sort = customSortMethods ?? throw new ArgumentNullException(nameof(customSortMethods));
        }

        /// <summary>
        /// Gets the custom filter method dictionary.
        /// </summary>
        public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>> Filter { get; }

        /// <summary>
        /// Gets the custom sort method dictionary.
        /// </summary>
        public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>> Sort { get; }
    }
}
