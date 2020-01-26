using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Sorting;
using System;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services
{
    /// <summary>
    /// Provides access for custom methods related services.
    /// </summary>
    public interface ICustomMethodsContext
    {
        /// <summary>
        /// Gets the custom filter method dictionary.
        /// </summary>
        IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>> Filter { get; }

        /// <summary>
        /// Gets the custom sort method dictionary.
        /// </summary>
        IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>> Sort { get; }
    }
}
