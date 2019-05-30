using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using Sieve.Sample.Entities;

namespace Sieve.Sample.Services
{
	public class ApplicationSieveProcessor : SieveProcessor
    {
        public ApplicationSieveProcessor(
            IOptions<SieveOptions> options,
            IFilterOperatorProvider filterOperatorProvider,
            IFilterTermParser filterTermParser,
            ISieveCustomSortMethods customSortMethods,
            ISieveCustomFilterMethods customFilterMethods)
            : base(options, filterOperatorProvider, filterTermParser, customSortMethods, customFilterMethods)
        {

        }

        protected override ISievePropertyMapper MapProperties(ISievePropertyMapper mapper)
        {
            mapper.Property<Post>(p => p.Title)
                .CanSort()
                .CanFilter()
                .HasName("CustomTitleName");

            return mapper;
        }
    }
}
