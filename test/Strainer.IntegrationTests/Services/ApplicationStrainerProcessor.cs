using Fluorite.Strainer.Services;
using Fluorite.Strainer.TestModels;

namespace Fluorite.Strainer.IntegrationTests.Services
{
    public class ApplicationStrainerProcessor : StrainerProcessor
    {
        public ApplicationStrainerProcessor(IStrainerContext context) : base(context)
        {

        }

        protected override IStrainerPropertyMapper MapProperties(IStrainerPropertyMapper mapper)
        {
            mapper.Property<Post>(p => p.ThisHasNoAttributeButIsAccessible)
                .CanSort()
                .CanFilter()
                .HasDisplayName("shortname");

            mapper.Property<Post>(p => p.TopComment.Text)
                .CanFilter();

            mapper.Property<Post>(p => p.TopComment.Id)
                .CanSort();

            mapper.Property<Post>(p => p.OnlySortableViaFluentApi)
                .CanSort();

            mapper.Property<Post>(p => p.TopComment.Text)
                .CanFilter()
                .HasDisplayName("topc");

            mapper.Property<Post>(p => p.FeaturedComment.Text)
                .CanFilter()
                .HasDisplayName("featc");

            return mapper;
        }
    }
}
