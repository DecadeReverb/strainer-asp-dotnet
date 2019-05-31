using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using Sieve.Services.Filtering;
using Sieve.UnitTests.Entities;

namespace Sieve.UnitTests.Services
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
            mapper.Property<Post>(p => p.ThisHasNoAttributeButIsAccessible)
                .CanSort()
                .CanFilter()
                .HasName("shortname");

            mapper.Property<Post>(p => p.TopComment.Text)
                .CanFilter();

            mapper.Property<Post>(p => p.TopComment.Id)
                .CanSort();

            mapper.Property<Post>(p => p.OnlySortableViaFluentApi)
                .CanSort();

            mapper.Property<Post>(p => p.TopComment.Text)
                .CanFilter()
                .HasName("topc");

            mapper.Property<Post>(p => p.FeaturedComment.Text)
                .CanFilter()
                .HasName("featc");

            return mapper;
        }
    }
}
