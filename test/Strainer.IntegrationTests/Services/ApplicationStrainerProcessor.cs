using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
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

        protected override void MapProperties(IMetadataMapper mapper)
        {
            mapper.Property<Post>(p => p.ThisHasNoAttributeButIsAccessible)
                .IsSortable()
                .IsFilterable()
                .HasDisplayName("shortname");

            mapper.Property<Post>(p => p.TopComment.Text)
                .IsFilterable();

            mapper.Property<Post>(p => p.TopComment.Id)
                .IsSortable()
                .IsDefaultSort();

            mapper.Property<Post>(p => p.OnlySortableViaFluentApi)
                .IsSortable();

            mapper.Property<Post>(p => p.TopComment.Text)
                .IsFilterable()
                .HasDisplayName("topc");

            mapper.Property<Post>(p => p.FeaturedComment.Text)
                .IsFilterable()
                .HasDisplayName("featc");

            mapper.Object<Comment>(comment => comment.Id)
                .IsFilterable()
                .IsSortable();
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
                ? context.OrderedSource
                    .ThenBy(p => p.LikeCount)
                    .ThenBy(p => p.CommentCount)
                    .ThenBy(p => p.DateCreated)
                : context.Source
                    .OrderBy(p => p.LikeCount)
                    .ThenBy(p => p.CommentCount)
                    .ThenBy(p => p.DateCreated);
        }
        #endregion
    }
}
