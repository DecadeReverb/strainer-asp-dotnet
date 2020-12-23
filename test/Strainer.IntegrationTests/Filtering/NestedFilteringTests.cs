using FluentAssertions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Filtering
{
    public class NestedFilteringTests : StrainerFixtureBase
    {
        public NestedFilteringTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void NestedFilteringWorks()
        {
            // Arrange
            var posts = new Post[]
            {
                new Post
                {
                    Comment = new Comment
                    {
                        Text = "Nice!"
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
                Filters = "Comment.Text==Nice!",
            };
            var processor = Factory.CreateDefaultProcessor<TestStrainerModule>();

            // Act
            var result = processor.ApplyFiltering(model, posts);

            // Assert
            result.Should().Contain(p => p.Comment.Text == "Nice!");
        }

        [StrainerObject(nameof(Id))]
        private class Post
        {
            public Comment Comment { get; set; }

            public int Id { get; set; }
        }

        [StrainerObject(nameof(Id))]
        private class Comment
        {
            public int Id { get; set; }

            public string Text { get; set; }
        }
    }
}
