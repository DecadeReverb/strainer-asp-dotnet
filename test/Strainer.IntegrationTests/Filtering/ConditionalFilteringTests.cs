using FluentAssertions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Filtering
{
    public class ConditionalFilteringTests : StrainerFixtureBase
    {
        public ConditionalFilteringTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void OrNameFiltering_Works()
        {
            // Arrange
            var posts = new Post[]
            {
                new Post
                {
                    CommentCount = 0,
                    LikeCount = 20,
                },
                new Post
                {
                    CommentCount = 20,
                    LikeCount = 0,
                },
                new Post
                {
                    CommentCount = 0,
                    LikeCount = 0,
                },
            }.AsQueryable();
            var model = new StrainerModel()
            {
                Filters = "(CommentCount|LikeCount)==20",
            };
            var processor = Factory.CreateDefaultProcessor<TestStrainerModule>();

            // Act
            var result = processor.Apply(model, posts);

            // Assert
            result.Should().OnlyContain(p => p.CommentCount == 20 || p.LikeCount == 20);
        }

        [Fact]
        public void OrValueFiltering_Works()
        {
            // Arrange
            var posts = new Post[]
            {
                new Post
                {
                    Title = "How not to die?",
                },
                new Post
                {
                    Title = "Why Java programmers can't C#",
                },
                new Post
                {
                    Title = "When the dinner will be ready?"
                },
            }.AsQueryable();
            var model = new StrainerModel()
            {
                Filters = "Title_=How|Why",
            };
            var processor = Factory.CreateDefaultProcessor<TestStrainerModule>();

            // Act
            var result = processor.Apply(model, posts);

            // Assert
            result.Should().OnlyContain(p => p.Title.StartsWith("How") || p.Title.StartsWith("Why"));
        }

        [Fact]
        public void OrValueFiltering_WithBraces_Work()
        {
            // Arrange
            var posts = new Post[]
            {
                new Post
                {
                    Title = ":)",
                },
                new Post
                {
                    Title = "(YES/NO)",
                },
                new Post
                {
                    Title = "",
                },
            }.AsQueryable();
            var model = new StrainerModel()
            {
                Filters = "Title@=(|)",
            };
            var processor = Factory.CreateDefaultProcessor<TestStrainerModule>();

            // Act
            var result = processor.Apply(model, posts);

            // Assert
            result.Should().OnlyContain(p => p.Title.Contains("(") || p.Title.Contains(")"));
        }

        [Fact]
        public void OrName_Works()
        {
            // Arrange
            var posts = new Post[]
            {
                new Post
                {
                    Id = 20,
                },
                new Post
                {
                    CommentCount = 20,
                },
                new Post
                {
                    LikeCount = 20,
                },
            }.AsQueryable();
            var model = new StrainerModel()
            {
                Filters = "(CommentCount|LikeCount)==20",
            };
            var processor = Factory.CreateDefaultProcessor<TestStrainerModule>();

            // Act
            var result = processor.Apply(model, posts);

            // Assert
            result.Should().OnlyContain(p => p.CommentCount == 20 || p.LikeCount == 20);
        }

        [Fact]
        public void OrName_Works_With_EmptyNameBeingIgnored()
        {
            // Arrange
            var posts = new Post[]
            {
                new Post
                {
                    Id = 20,
                },
                new Post
                {
                    CommentCount = 20,
                },
                new Post
                {
                    LikeCount = 20,
                },
            }.AsQueryable();
            var model = new StrainerModel()
            {
                Filters = "(CommentCount|)==20",
            };
            var processor = Factory.CreateDefaultProcessor<TestStrainerModule>();

            // Act
            var result = processor.Apply(model, posts);

            // Assert
            result.Should().OnlyContain(p => p.CommentCount == 20);
        }

        [StrainerObject(nameof(Id))]
        private class Post
        {
            public int CommentCount { get; set; }

            public int Id { get; set; }

            public int LikeCount { get; set; }

            public string Title { get; set; }
        }
    }
}
