using Fluorite.Strainer.Services.Modules;
using Fluorite.Strainer.TestModels;
using System.Linq;

namespace Fluorite.Strainer.IntegrationTests.Fixtures
{
    public class TestStrainerModule : StrainerModule
    {
        public TestStrainerModule()
        {

        }

        public override void Load(IStrainerModuleBuilder builder)
        {
            builder.AddCustomFilterMethod<Comment>(nameof(TestComment))
                .HasFunction(TestComment);

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

        private IQueryable<Comment> TestComment(IQueryable<Comment> source, string filterOperator)
        {
            return source;
        }

        #endregion
    }
}
