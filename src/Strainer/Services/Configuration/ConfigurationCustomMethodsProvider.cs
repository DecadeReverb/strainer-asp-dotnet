using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Sorting;
using System;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Configuration
{
    public class ConfigurationCustomMethodsProvider : IConfigurationCustomMethodsProvider
    {
        private readonly IStrainerConfigurationProvider _strainerConfigurationProvider;

        public ConfigurationCustomMethodsProvider(IStrainerConfigurationProvider strainerConfigurationProvider)
        {
            _strainerConfigurationProvider = strainerConfigurationProvider
                ?? throw new ArgumentNullException(nameof(strainerConfigurationProvider));
        }

        /// <summary>
        /// Gets the object custom filter methods dictionary.
        /// </summary>
        public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>> GetCustomFilterMethods()
        {
            return _strainerConfigurationProvider
                .GetStrainerConfiguration()
                .CustomFilterMethods;
        }

        /// <summary>
        /// Gets the object custom sorting methods dictionary.
        /// </summary>
        public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>> GetCustomSortMethods()
        {
            return _strainerConfigurationProvider
                .GetStrainerConfiguration()
                .CustomSortMethods;
        }
    }
}
