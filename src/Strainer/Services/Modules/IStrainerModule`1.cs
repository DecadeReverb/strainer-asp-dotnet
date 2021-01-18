using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Modules
{
    /// <summary>
    /// Stores Strainer configuration in an encapsulated form of a module.
    /// </summary>
    /// <typeparam name="T">
    /// The type of model for which this module is for.
    /// </typeparam>
    public interface IStrainerModule<T>
    {
        /// <summary>
        /// Gets the object custom filter methods dictionary.
        /// </summary>
        IDictionary<string, ICustomFilterMethod> CustomFilterMethods { get; }

        /// <summary>
        /// Gets the object custom sorting methods dictionary.
        /// </summary>
        IDictionary<string, ICustomSortMethod> CustomSortMethods { get; }

        /// <summary>
        /// Gets the default metadata.
        /// </summary>
        IPropertyMetadata DefaultMetadata { get; }

        /// <summary>
        /// Gets the object filter operator dictionary.
        /// </summary>
        IDictionary<string, IFilterOperator> FilterOperators { get; }

        /// <summary>
        /// Gets the object metadata.
        /// </summary>
        IObjectMetadata ObjectMetadata { get; }

        /// <summary>
        /// Gets the object property dictionary.
        /// </summary>
        IDictionary<string, IPropertyMetadata> PropertyMetadata { get; }
    }
}
