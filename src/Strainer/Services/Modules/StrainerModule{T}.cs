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
    /// <typeparam name="T">
    /// The type of model for which this module is for.
    /// </typeparam>
    public abstract class StrainerModule<T> : IStrainerModule<T>, IStrainerModule
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StrainerModule{T}"/> class.
        /// </summary>
        protected StrainerModule()
        {
            CustomFilterMethods = new Dictionary<Type, IDictionary<string, ICustomFilterMethod>>();
            CustomSortMethods = new Dictionary<Type, IDictionary<string, ICustomSortMethod>>();
            DefaultMetadata = new Dictionary<Type, IPropertyMetadata>();
            FilterOperators = new Dictionary<string, IFilterOperator>();
            ObjectMetadata = new Dictionary<Type, IObjectMetadata>();
            PropertyMetadata = new Dictionary<Type, IDictionary<string, IPropertyMetadata>>();
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
        /// Gets the object metadata dictionary.
        /// </summary>
        public IDictionary<Type, IObjectMetadata> ObjectMetadata { get; }

        /// <summary>
        /// Gets the object property dictionary.
        /// </summary>
        public IDictionary<Type, IDictionary<string, IPropertyMetadata>> PropertyMetadata { get; }

        /// <summary>
        /// Loads configuration in module.
        /// <para/>
        /// Override this method to specify configuration for this module.
        /// </summary>
        /// <param name="builder">
        /// The Strainer Module builder.
        /// </param>
        public abstract void Load(IStrainerModuleBuilder<T> builder);

        /// <summary>
        /// Loads configuration in module.
        /// <para/>
        /// Override this method to specify configuration for this module.
        /// </summary>
        /// <param name="builder">
        /// The Strainer Module builder.
        /// </param>
        public void Load(IStrainerModuleBuilder builder)
        {
            if (builder is null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            if (builder is not IStrainerModuleBuilder<T> typedBuilder)
            {
                throw new NotSupportedException("Only strongly typed module builder is supported.");
            }

            Load(typedBuilder);
        }
    }
}
