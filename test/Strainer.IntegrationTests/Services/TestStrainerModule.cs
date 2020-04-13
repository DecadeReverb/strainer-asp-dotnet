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

        public override void Load(IStrainerModuleBuilder builder)
        {
            builder.AddCustomFilterMethod<Post>(nameof(IsPopular))
               .HasFunction(IsPopular);
            builder.AddCustomFilterMethod<Post>(nameof(HasInTitleFilterOperator))
                .HasFunction(HasInTitleFilterOperator);
            builder.AddCustomFilterMethod<Comment>(nameof(IsNew))
                .HasFunction(IsNew);
            builder.AddCustomFilterMethod<Comment>(nameof(TestComment))
                .HasFunction(TestComment);

            builder.AddCustomSortMethod<Post>(nameof(Popularity))
                .HasFunction(Popularity);

            builder.AddProperty<Post>(p => p.ThisHasNoAttributeButIsAccessible)
                .IsSortable()
                .IsFilterable()
                .HasDisplayName("shortname");

            builder.AddProperty<Post>(p => p.TopComment.Text)
                .IsFilterable();

            builder.AddProperty<Post>(p => p.TopComment.Id)
                .IsSortable()
                .IsDefaultSort();

            builder.AddProperty<Post>(p => p.OnlySortableViaFluentApi)
                .IsSortable();

            builder.AddProperty<Post>(p => p.TopComment.Text)
                .IsFilterable()
                .HasDisplayName("topc");

            builder.AddProperty<Post>(p => p.FeaturedComment.Text)
                .IsFilterable()
                .HasDisplayName("featc");

            builder.AddObject<Comment>(comment => comment.Id)
                .IsFilterable()
                .IsSortable();
        }

        #region custom filter methods
        private IQueryable<Post> IsPopular(IQueryable<Post> source, string filterOperator)
        {
            return source.Where(p => p.LikeCount > 100);
        }

        private IQueryable<Post> HasInTitleFilterOperator(IQueryable<Post> source, string filterOperator)
        {
            return source.Where(p => p.Title.Contains(filterOperator));
        }

        private IQueryable<Comment> IsNew(IQueryable<Comment> source, string filterOperator)
        {
            return source.Where(c => c.DateCreated > DateTimeOffset.UtcNow.AddDays(-2));
        }

        private IQueryable<Comment> TestComment(IQueryable<Comment> source, string filterOperator)
        {
            return source;
        }
        #endregion

        #region custom sort methods
        private IOrderedQueryable<Post> Popularity(IQueryable<Post> source, bool isDescending, bool isSubsequent)
        {
            return isSubsequent
                ? (source as IOrderedQueryable<Post>)
                    .ThenBy(p => p.LikeCount)
                    .ThenBy(p => p.CommentCount)
                    .ThenBy(p => p.DateCreated)
                : source
                    .OrderBy(p => p.LikeCount)
                    .ThenBy(p => p.CommentCount)
                    .ThenBy(p => p.DateCreated);
        }
        #endregion
    }
}
