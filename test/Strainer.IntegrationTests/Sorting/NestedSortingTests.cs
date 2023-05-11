using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.IntegrationTests.Sorting
{
    public class NestedSortingTests : StrainerFixtureBase
    {
        public NestedSortingTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void NestedSortsWorks_For_Int()
        {
            // Arrange
            var posts = new Post[]
            {
                new Post
                {
                    Comment = new Comment
                    {
                        Id = 34,
                    },
                },
                new Post
                {
                    Comment = new Comment
                    {
                        Id = 11,
                    },
                },
            }.AsQueryable();
            var model = new StrainerModel()
            {
                Sorts = "Comment.Id",
            };
            var processor = Factory.CreateDefaultProcessor<TestStrainerModule>();

            // Act
            var result = processor.ApplySorting(model, posts);

            // Assert
            result.Should().BeInAscendingOrder(p => p.Comment.Id);
        }

        [Fact]
        public void NestedSortsWorks_For_String()
        {
            // Arrange
            var posts = new Post[]
            {
                new Post
                {
                    Comment = new Comment
                    {
                        Text = "Nice!",
                    },
                },
                new Post
                {
                    Comment = new Comment
                    {
                        Text = "Good job!",
                    },
                },
            }.AsQueryable();
            var model = new StrainerModel()
            {
                Sorts = "Comment.Text",
            };
            var processor = Factory.CreateDefaultProcessor<TestStrainerModule>();

            // Act
            var result = processor.ApplySorting(model, posts);

            // Assert
            result.Should().BeInAscendingOrder(p => p.Comment.Text);
        }

        private class TestStrainerModule : StrainerModule
        {
            public override void Load(IStrainerModuleBuilder strainerModuleBuilder)
            {
                strainerModuleBuilder
                    .AddProperty<Post>(p => p.Comment.Id)
                    .IsSortable()
                    .IsDefaultSort();
                strainerModuleBuilder
                    .AddProperty<Post>(p => p.Comment.Text)
                    .IsSortable();
            }
        }

        private class Post
        {
            public Comment Comment { get; set; }

            public int Id { get; set; }
        }

        private class Comment
        {
            public int Id { get; set; }

            public string Text { get; set; }
        }
    }
}
