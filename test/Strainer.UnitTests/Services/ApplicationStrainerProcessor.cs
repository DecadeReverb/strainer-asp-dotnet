using Fluorite.Strainer.Services;
using Fluorite.Strainer.UnitTests.Entities;

namespace Fluorite.Strainer.UnitTests.Services
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
