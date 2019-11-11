﻿using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;
using System;

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
            IStrainerOptionsProvider optionsProvider,
            IFilterContext filteringContext,
            ISortingContext sortingContext,
            IPropertyMapper mapper,
            IAttributeMetadataProvider metadataProvider,
            ICustomMethodsContext customMethodsContext)
        {
            CustomMethods = customMethodsContext ?? throw new ArgumentNullException(nameof(customMethodsContext));
            Filter = filteringContext ?? throw new ArgumentNullException(nameof(filteringContext));
            Sorting = sortingContext ?? throw new ArgumentNullException(nameof(sortingContext));
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            AttributeMetadataProvider = metadataProvider ?? throw new ArgumentNullException(nameof(metadataProvider));
            Options = (optionsProvider ?? throw new ArgumentNullException(nameof(optionsProvider)))
                .GetStrainerOptions();
        }

        /// <summary>
        /// Gets the context for custom methods.
        /// </summary>
        public ICustomMethodsContext CustomMethods { get; }

        /// <summary>
        /// Gets the filtering context.
        /// </summary>
        public IFilterContext Filter { get; }

        /// <summary>
        /// Gets the property mapper.
        /// </summary>
        public IPropertyMapper Mapper { get; }

        /// <summary>
        /// Gets the property metadata provider.
        /// </summary>
        public IAttributeMetadataProvider AttributeMetadataProvider { get; }

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
