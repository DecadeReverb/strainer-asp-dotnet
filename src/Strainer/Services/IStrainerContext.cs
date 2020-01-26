using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.Services
{
    /// <summary>
    /// Represents wrapper over main services used by Strainer.
    /// </summary>
    public interface IStrainerContext
    {
        /// <summary>
        /// Gets the context for custom methods.
        /// </summary>
        ICustomMethodsContext CustomMethods { get; }

        /// <summary>
        /// Gets the filtering context.
        /// </summary>
        IFilterContext Filter { get; }

        /// <summary>
        /// Gets the metadata providers facade.
        /// </summary>
        IMetadataFacade Metadata { get; }

        /// <summary>
        /// Gets the Strainer options.
        /// </summary>
        StrainerOptions Options { get; }

        /// <summary>
        /// Gets the sorting context.
        /// </summary>
        ISortingContext Sorting { get; }
    }
}
