using Fluorite.Strainer.ExampleWebApi.Entities;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;
using System.Linq;
using System.Linq.Expressions;

namespace Fluorite.Strainer.ExampleWebApi.Services
{
    public class ApplicationStrainerProcessor : StrainerProcessor
    {
        public ApplicationStrainerProcessor(IStrainerContext context) : base(context)
        {

        }

        protected override void MapCustomFilterMethods(ICustomFilterMethodMapper mapper)
        {
            mapper.CustomMethod<Post>(nameof(IsNew))
                .WithFunction(IsNew);
        }

        protected override void MapCustomSortMethods(ICustomSortMethodMapper mapper)
        {
            mapper.CustomMethod<Post>(nameof(Popularity))
                .WithFunction(Popularity);
        }

        protected override void MapFilterOperators(IFilterOperatorMapper mapper)
        {
            mapper.Operator(symbol: "!=*")
                .HasName("not equal to (case insensitive)")
                .HasExpression((context) => Expression.NotEqual(context.FilterValue, context.PropertyValue))
                .IsCaseInsensitive();
        }

        protected override void MapProperties(IPropertyMapper mapper)
        {

            mapper.Property<Post>(p => p.Title)
                .CanSort()
                .CanFilter()
                .HasDisplayName("CustomTitleName");
        }

        private IQueryable<Post> IsNew(ICustomFilterMethodContext<Post> context)
            => context.Source.Where(p => p.LikeCount < 100 && p.CommentCount < 5);

        private IOrderedQueryable<Post> Popularity(ICustomSortMethodContext<Post> context)
        {
            return context.IsSubsequent
                ? (context.Source as IOrderedQueryable<Post>).ThenBy(p => p.LikeCount)
                : context.Source.OrderBy(p => p.LikeCount)
                    .ThenBy(p => p.CommentCount)
                    .ThenBy(p => p.DateCreated);
        }
    }
}
