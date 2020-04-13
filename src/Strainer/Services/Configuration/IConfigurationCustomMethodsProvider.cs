using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Sorting;
using System;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Configuration
{
    public interface IConfigurationCustomMethodsProvider
    {
        /// <summary>
        /// Gets the object custom filter methods dictionary.
        /// </summary>
        IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>> GetCustomFilterMethods();

        /// <summary>
        /// Gets the object custom sorting methods dictionary.
        /// </summary>
        IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>> GetCustomSortMethods();
    }
}
