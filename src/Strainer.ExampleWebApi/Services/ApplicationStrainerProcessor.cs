using Fluorite.Strainer.ExampleWebApi.Entities;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using System.Linq.Expressions;

namespace Fluorite.Strainer.ExampleWebApi.Services
{
    public class ApplicationStrainerProcessor : StrainerProcessor
    {
        public ApplicationStrainerProcessor(IStrainerContext context) : base(context)
        {

        }

        protected override IFilterOperatorMapper MapFilterOperators(IFilterOperatorMapper mapper)
        {
            mapper.Operator(symbol: "!=*")
                .HasName("not equal to (case insensitive)")
                .HasExpression((context) => Expression.NotEqual(context.FilterValue, context.PropertyValue))
                .IsCaseInsensitive();

            return mapper;
        }

        protected override IStrainerPropertyMapper MapProperties(IStrainerPropertyMapper mapper)
        {
            mapper.Property<Post>(p => p.Title)
                .CanSort()
                .CanFilter()
                .HasDisplayName("CustomTitleName");

            return mapper;
        }
    }
}
