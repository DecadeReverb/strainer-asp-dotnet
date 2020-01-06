using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services.Modules;
using Fluorite.Strainer.TestModels;
using System;
using System.Linq;

namespace Fluorite.Strainer.IntegrationTests.Services
{
    public class TestStrainerModule : StrainerModule
    {
        public TestStrainerModule()
        {

        }

        public override void Load()
        {
            AddCustomFilterMethod<Post>(nameof(IsPopular))
               .HasExpression(IsPopular);
            AddCustomFilterMethod<Post>(nameof(HasInTitle))
                .HasExpression(HasInTitle);
            AddCustomFilterMethod<Comment>(nameof(IsNew))
                .HasExpression(IsNew);
            AddCustomFilterMethod<Comment>(nameof(TestComment))
                .HasExpression(TestComment);

            AddCustomSortMethod<Post>(nameof(Popularity))
                .HasExpression(Popularity);

            AddProperty<Post>(p => p.ThisHasNoAttributeButIsAccessible)
                .IsSortable()
                .IsFilterable()
                .HasDisplayName("shortname");

            AddProperty<Post>(p => p.TopComment.Text)
                .IsFilterable();

            AddProperty<Post>(p => p.TopComment.Id)
                .IsSortable()
                .IsDefaultSort();

            AddProperty<Post>(p => p.OnlySortableViaFluentApi)
                .IsSortable();

            AddProperty<Post>(p => p.TopComment.Text)
                .IsFilterable()
                .HasDisplayName("topc");

            AddProperty<Post>(p => p.FeaturedComment.Text)
                .IsFilterable()
                .HasDisplayName("featc");

            AddObject<Comment>(comment => comment.Id)
                .IsFilterable()
                .IsSortable();
        }

        #region custom filter methods
        private IQueryable<Post> IsPopular(ICustomFilterMethodContext<Post> context)
        {
            return context.Source.Where(p => p.LikeCount > 100);
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
