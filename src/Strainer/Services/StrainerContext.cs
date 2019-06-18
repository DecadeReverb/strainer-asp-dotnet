﻿using Microsoft.Extensions.Options;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.Services
{
    public class StrainerContext : IStrainerContext
    {
        public StrainerContext(
            IOptions<StrainerOptions> options,
            IFilteringContext filteringContext,
            ISortingContext sortingContext,
            IStrainerPropertyMapper mapper,
            ICustomMethodsContext customMethodsContext)
        {
            CustomMethods = customMethodsContext;
            Filtering = filteringContext;
            Sorting = sortingContext;
            Mapper = mapper;
            Options = options.Value;
        }

        public ICustomMethodsContext CustomMethods { get; }

        public IFilteringContext Filtering { get; }

        public IStrainerPropertyMapper Mapper { get; }

        public StrainerOptions Options { get; }

        public ISortingContext Sorting { get; }
    }
}
