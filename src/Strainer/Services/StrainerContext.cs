using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Configuration;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.Services
{
    /// <summary>
    /// Represents wrapper over main services used by Strainer.
    /// </summary>
    public class StrainerContext : IStrainerContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StrainerContext"/> class.
        /// </summary>
        public StrainerContext(
            IConfigurationCustomMethodsProvider customMethodsConfigurationProvider,
            IStrainerOptionsProvider optionsProvider,
            IFilterContext filteringContext,
            ISortingContext sortingContext,
            IMetadataFacade metadataProvidersFacade)
        {
            CustomMethods = customMethodsConfigurationProvider
                ?? throw new ArgumentNullException(nameof(customMethodsConfigurationProvider));
            Filter = filteringContext ?? throw new ArgumentNullException(nameof(filteringContext));
            Sorting = sortingContext ?? throw new ArgumentNullException(nameof(sortingContext));
            Metadata = metadataProvidersFacade ?? throw new ArgumentNullException(nameof(metadataProvidersFacade));
            Options = (optionsProvider ?? throw new ArgumentNullException(nameof(optionsProvider)))
                .GetStrainerOptions();
        }

        /// <summary>
        /// Gets the custom methods provider.
        /// </summary>
        public IConfigurationCustomMethodsProvider CustomMethods { get; }

        /// <summary>
        /// Gets the filtering context.
        /// </summary>
        public IFilterContext Filter { get; }

        /// <summary>
        /// Gets the property metadata provider.
        /// </summary>
        public IMetadataFacade Metadata { get; }

        /// <summary>
        /// Gets the Strainer options.
        /// </summary>
        public StrainerOptions Options { get; }

        /// <summary>
        /// Gets the sorting context.
        /// </summary>
        public ISortingContext Sorting { get; }
    }
}
