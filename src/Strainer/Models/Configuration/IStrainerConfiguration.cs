using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;

namespace Fluorite.Strainer.Models.Configuration
{
    /// <summary>
    /// Provides readonly information about metadata, filter operators
    /// and custom methods for Strainer.
    /// </summary>
    public interface IStrainerConfiguration
    {
        /// <summary>
        /// Gets the object custom filter methods dictionary.
        /// </summary>
        IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>> CustomFilterMethods { get; }

        /// <summary>
        /// Gets the object custom sorting methods dictionary.
        /// </summary>
        IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>> CustomSortMethods { get; }

        /// <summary>
        /// Gets the object default dictionary.
        /// </summary>
        IReadOnlyDictionary<Type, IPropertyMetadata> DefaultMetadata { get; }

        /// <summary>
        /// Gets the object filter operator dictionary.
        /// </summary>
        IReadOnlyDictionary<string, IFilterOperator> FilterOperators { get; }

        /// <summary>
        /// Gets the object property dictionary.
        /// </summary>
        IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> PropertyMetadata { get; }

        /// <summary>
        /// Gets the object metadata dictionary.
        /// </summary>
        IReadOnlyDictionary<Type, IObjectMetadata> ObjectMetadata { get; }
    }
}
