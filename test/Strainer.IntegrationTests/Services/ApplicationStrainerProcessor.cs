using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;
using Fluorite.Strainer.TestModels;
using System;
using System.Linq;

namespace Fluorite.Strainer.IntegrationTests.Services
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
            mapper.CustomMethod<Post>(nameof(HasInTitle))
                .WithFunction(HasInTitle);
            mapper.CustomMethod<Comment>(nameof(IsNew))
                .WithFunction(IsNew);
            mapper.CustomMethod<Comment>(nameof(TestComment))
                .WithFunction(TestComment);
        }

        protected override void MapCustomSortMethods(ICustomSortMethodMapper mapper)
        {
            mapper.CustomMethod<Post>(nameof(Popularity))
                .WithFunction(Popularity);
        }

        protected override void MapProperties(IStrainerPropertyMapper mapper)
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
        }

        #region custom filter methods
        private IQueryable<Post> IsNew(ICustomFilterMethodContext<Post> context)
        {
            return context.Source.Where(p => p.LikeCount < 100);
        }

        private IQueryable<Post> HasInTitle(ICustomFilterMethodContext<Post> context)
        {
            return context.Source.Where(p => p.Title.Contains(context.Term.Values[0]));
        }

        private IQueryable<Comment> IsNew(ICustomFilterMethodContext<Comment> context)
        {
            return context.Source.Where(c => c.DateCreated > DateTimeOffset.UtcNow.AddDays(-2));
        }

        private IQueryable<Comment> TestComment(ICustomFilterMethodContext<Comment> context)
        {
            return context.Source;
        }
        #endregion

        #region custom sort methods
        private IOrderedQueryable<Post> Popularity(ICustomSortMethodContext<Post> context)
        {
            return context.IsSubsequent
                ? (context.Source as IOrderedQueryable<Post>).ThenBy(p => p.LikeCount)
                : context.Source.OrderBy(p => p.LikeCount)
                    .ThenBy(p => p.CommentCount)
                    .ThenBy(p => p.DateCreated);
        }
        #endregion
    }
}
