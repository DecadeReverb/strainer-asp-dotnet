﻿using Microsoft.Extensions.Options;
using Sieve.Models;

namespace Sieve.Services
{
    public class SieveProcessor : SieveProcessor<SieveModel, FilterTerm, SortTerm>, ISieveProcessor
    {
        public SieveProcessor(IOptions<SieveOptions> options) : base(options)
        {

        }

        public SieveProcessor(IOptions<SieveOptions> options, ISieveCustomSortMethods customSortMethods)
            : base(options, customSortMethods)
        {

        }

        public SieveProcessor(IOptions<SieveOptions> options, ISieveCustomFilterMethods customFilterMethods)
            : base(options, customFilterMethods)
        {

        }

        public SieveProcessor(IOptions<SieveOptions> options, ISieveCustomSortMethods customSortMethods, ISieveCustomFilterMethods customFilterMethods)
            : base(options, customSortMethods, customFilterMethods)
        {

        }
    }
}
