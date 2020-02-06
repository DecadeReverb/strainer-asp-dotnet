using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;
using System;
using System.Collections.Generic;

namespace Fluorite.Strainer.Services.Modules
{
    /// <summary>
    /// Provides configuration information about metadata, filter operators
    /// and custom methods for Strainer.
    /// </summary>
    public abstract class StrainerModule : IStrainerModule
    {
        /// <summary>
        /// Initializes new instance of <see cref="StrainerModule"/> class.
        /// </summary>
        protected StrainerModule()
        {
            CustomFilterMethods = new Dictionary<Type, IDictionary<string, ICustomFilterMethod>>();
            CustomSortMethods = new Dictionary<Type, IDictionary<string, ICustomSortMethod>>();
            DefaultMetadata = new Dictionary<Type, IPropertyMetadata>();
            FilterOperators = new Dictionary<string, IFilterOperator>();
            PropertyMetadata = new Dictionary<Type, IDictionary<string, IPropertyMetadata>>();
            ObjectMetadata = new Dictionary<Type, IObjectMetadata>();
        }

        /// <summary>
        /// Gets the object custom filter methods dictionary.
        /// </summary>
        public IDictionary<Type, IDictionary<string, ICustomFilterMethod>> CustomFilterMethods { get; }

        /// <summary>
        /// Gets the object custom sorting methods dictionary.
        /// </summary>
        public IDictionary<Type, IDictionary<string, ICustomSortMethod>> CustomSortMethods { get; }

        /// <summary>
        /// Gets the object default dictionary.
        /// </summary>
        public IDictionary<Type, IPropertyMetadata> DefaultMetadata { get; }

        /// <summary>
        /// Gets the object filter operator dictionary.
        /// </summary>
        public IDictionary<string, IFilterOperator> FilterOperators { get; }

        /// <summary>
        /// Gets the object property dictionary.
        /// </summary>
        public IDictionary<Type, IDictionary<string, IPropertyMetadata>> PropertyMetadata { get; }

        /// <summary>
        /// Gets the object metadata dictionary.
        /// </summary>
        public IDictionary<Type, IObjectMetadata> ObjectMetadata { get; }

        /// <summary>
        /// Loads configuration in module.
        /// <para/>
        /// Override this method to specify configuration for this module.
        /// </summary>
        /// <param name="strainerModuleBuilder">
        /// The Strainer Module builder.
        /// </param>
        public virtual void Load(IStrainerModuleBuilder strainerModuleBuilder)
        {

        }
    }
}
