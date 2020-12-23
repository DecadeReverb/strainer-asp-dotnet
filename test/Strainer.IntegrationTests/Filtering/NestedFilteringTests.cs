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
            var processor = Factory.CreateDefaultProcessor();

            // Act
            var result = processor.ApplyFiltering(model, posts);

            // Assert
            result.Should().Contain(p => p.Comment.Text == "Nice!");
        }

        [StrainerObject(nameof(Comment))]
        private class Post
        {
            public Comment Comment { get; set; }
        }

        [StrainerObject(nameof(Text))]
        private class Comment
        {
            public string Text { get; set; }
        }
    }
}
