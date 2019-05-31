using Sieve.Sample.Entities;
using Sieve.Services;

namespace Sieve.Sample.Services
{
    public class ApplicationSieveProcessor : SieveProcessor
    {
        public ApplicationSieveProcessor(ISieveContext context) : base(context)
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
