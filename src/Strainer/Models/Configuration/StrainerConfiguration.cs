using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;
using System;
using System.Collections.Generic;

namespace Fluorite.Strainer.Models.Configuration
{
    /// <summary>
    /// Provides readonly information about metadata, filter operators
    /// and custom methods for Strainer.
    /// </summary>
    public class StrainerConfiguration : IStrainerConfiguration
    {
        /// <summary>
        /// Initializes new instance of <see cref="StrainerConfiguration"/> class.
        /// </summary>
        public StrainerConfiguration(
            IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>> customFilterMethods,
            IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>> customSortMethods,
            IReadOnlyDictionary<Type, IPropertyMetadata> defaultMetadata,
            IReadOnlyDictionary<string, IFilterOperator> filterOperators,
            IReadOnlyDictionary<Type, IObjectMetadata> objectMetadata,
            IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> propertyMetadata)
        {
            CustomFilterMethods = customFilterMethods ?? throw new ArgumentNullException(nameof(customFilterMethods));
            CustomSortMethods = customSortMethods ?? throw new ArgumentNullException(nameof(customSortMethods));
            DefaultMetadata = defaultMetadata ?? throw new ArgumentNullException(nameof(defaultMetadata));
            FilterOperators = filterOperators ?? throw new ArgumentNullException(nameof(filterOperators));
            ObjectMetadata = objectMetadata ?? throw new ArgumentNullException(nameof(objectMetadata));
            PropertyMetadata = propertyMetadata ?? throw new ArgumentNullException(nameof(propertyMetadata));
        }

        /// <summary>
        /// Gets the object custom filter methods dictionary.
        /// </summary>
        public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>> CustomFilterMethods { get; }

        /// <summary>
        /// Gets the object custom sorting methods dictionary.
        /// </summary>
        public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>> CustomSortMethods { get; }

        /// <summary>
        /// Gets the object default dictionary.
        /// </summary>
        public IReadOnlyDictionary<Type, IPropertyMetadata> DefaultMetadata { get; }

        /// <summary>
        /// Gets the object filter operator dictionary.
        /// </summary>
        public IReadOnlyDictionary<string, IFilterOperator> FilterOperators { get; }

        /// <summary>
        /// Gets the object metadata dictionary.
        /// </summary>
        public IReadOnlyDictionary<Type, IObjectMetadata> ObjectMetadata { get; }

        /// <summary>
        /// Gets the object property dictionary.
        /// </summary>
        public IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>> PropertyMetadata { get; }
    }
}
