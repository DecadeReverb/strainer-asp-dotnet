using Fluorite.Strainer.Example.Entities;
using Fluorite.Strainer.Services;

namespace Fluorite.Strainer.Example.Services
{
    public class ApplicationStrainerProcessor : StrainerProcessor
    {
        public ApplicationStrainerProcessor(IStrainerContext context) : base(context)
        {

        }

        protected override IStrainerPropertyMapper MapProperties(IStrainerPropertyMapper mapper)
        {
            mapper.Property<Post>(p => p.Title)
                .CanSort()
                .CanFilter()
                .HasName("CustomTitleName");

            return mapper;
        }
    }
}
