using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;

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
        ICustomFilterMethodDictionary Filter { get; }

        /// <summary>
        /// Gets the custom sort method dictionary.
        /// </summary>
        ICustomSortMethodDictionary Sort { get; }
    }
}
