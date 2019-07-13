using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.Services
{
    /// <summary>
    /// Represents wrapper over main services used by Strainer.
    /// </summary>
    public class StrainerContext : IStrainerContext
    {
        /// <summary>
        /// Initializes new instance of <see cref="StrainerContext"/> class.
        /// </summary>
        public StrainerContext(
            StrainerOptions options,
            IFilteringContext filteringContext,
            ISortingContext sortingContext,
            IStrainerPropertyMapper mapper,
            IStrainerPropertyMetadataProvider metadataProvider,
            ICustomMethodsContext customMethodsContext)
        {
            CustomMethods = customMethodsContext;
            Filtering = filteringContext;
            Sorting = sortingContext;
            Mapper = mapper;
            MetadataProvider = metadataProvider;
            Options = options;
        }

        /// <summary>
        /// Gets the context for custom methods.
        /// </summary>
        public ICustomMethodsContext CustomMethods { get; }

        /// <summary>
        /// Gets the filtering context.
        /// </summary>
        public IFilteringContext Filtering { get; }

        /// <summary>
        /// Gets the property mapper.
        /// </summary>
        public IStrainerPropertyMapper Mapper { get; }

        /// <summary>
        /// Gets the property metadata provider.
        /// </summary>
        public IStrainerPropertyMetadataProvider MetadataProvider { get; }

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
