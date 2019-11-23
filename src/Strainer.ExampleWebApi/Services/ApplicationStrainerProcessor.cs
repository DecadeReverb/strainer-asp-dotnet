using Fluorite.Strainer.ExampleWebApi.Entities;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Sorting;
using System;
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
            mapper.Operator(symbol: "=_")
                .HasName("ends with")
                .HasExpression((context) => Expression.Call(
                    context.PropertyValue,
                    typeof(string).GetMethod(nameof(string.EndsWith), new Type[] { typeof(string) }),
                    context.FilterValue))
                .IsStringBased();
        }

        protected override void MapProperties(IPropertyMetadataMapper mapper)
        {
            mapper.Property<Post>(p => p.Comments.Count)
                .IsFilterable()
                .IsSortable();
        }

        private IQueryable<Post> IsNew(ICustomFilterMethodContext<Post> context)
            => context.Source.Where(p => p.LikeCount < 100 && p.Comments.Count < 5);

        private IOrderedQueryable<Post> Popularity(ICustomSortMethodContext<Post> context)
        {
            return context.IsSubsequent
                ? (context.Source as IOrderedQueryable<Post>).ThenBy(p => p.LikeCount)
                : context.Source.OrderBy(p => p.LikeCount)
                    .ThenBy(p => p.Comments.Count)
                    .ThenByDescending(p => p.DateCreated);
        }
    }
}
