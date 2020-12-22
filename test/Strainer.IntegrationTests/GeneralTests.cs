using FluentAssertions;
using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.TestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests
{
    public class GeneralTests : StrainerFixtureBase
    {
        private readonly IQueryable<Post> _posts;
        private readonly IQueryable<Comment> _comments;

        public GeneralTests(StrainerFactory factory) : base(factory)
        {
            _comments = new List<Comment>
            {
                new Comment() {
                    Id = 0,
                    DateCreated = DateTimeOffset.UtcNow.AddDays(-20),
                    Text = "This is an old comment."
                },
                new Comment() {
                    Id = 1,
                    DateCreated = DateTimeOffset.UtcNow.AddDays(-1),
                    Text = "This is a fairly new comment. text"
                },
                new Comment() {
                    Id = 2,
                    DateCreated = DateTimeOffset.UtcNow,
                    Text = "This is a brand new comment. (Text in braces)"
                },
            }.AsQueryable();

            _posts = new List<Post>
            {
                new Post() {
                    Id = 0,
                    Title = "A==",
                    LikeCount = 200,
                    IsDraft = true,
                    CategoryId = 2,
                    TopComment = _comments.ElementAt(0),
                    FeaturedComment = _comments.ElementAt(0)
                },
                new Post() {
                    Id = 1,
                    Title = "B",
                    LikeCount = 50,
                    IsDraft = false,
                    CategoryId = 1,
                    TopComment = _comments.ElementAt(1),
                    FeaturedComment = _comments.ElementAt(1)
                },
                new Post() {
                    Id = 2,
                    Title = "C",
                    LikeCount = 0,
                    CategoryId = null,
                    TopComment = _comments.ElementAt(2),
                    FeaturedComment = _comments.ElementAt(2)
                },
                new Post() {
                    Id = 3,
                    Title = "D",
                    LikeCount = 3,
                    IsDraft = true,
                    CategoryId = 2,
                    TopComment = new Comment { Id = 1, Text = "D1" },
                    FeaturedComment = new Comment { Id = 7, Text = "D2" }
                },
            }.AsQueryable();
        }

        [Fact]
        public void ContainsCanBeCaseInsensitive()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "Title@=*a"
            };
            var processor = Factory.CreateDefaultProcessor<TestStrainerModule>();

            // Act
            var result = processor.Apply(model, _posts);

            // Assert
            result.Should().OnlyContain(p => p.Title.Contains("a", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void NotContainsWorks()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "Title!@=D",
            };
            var processor = Factory.CreateDefaultProcessor<TestStrainerModule>();

            // Act
            var result = processor.Apply(model, _posts);

            // Assert
            result.Should().OnlyContain(p => !p.Title.Contains("D", StringComparison.Ordinal));
        }

        [Fact]
        public void IsFilterableBools()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "IsDraft==false"
            };
            var processor = Factory.CreateDefaultProcessor<TestStrainerModule>();

            // Act
            var result = processor.ApplyFiltering(model, _posts);

            // Assert
            result.Should().OnlyContain(p => !p.IsDraft);
        }

        [Fact]
        public void IsSortableBools()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Sorts = "-IsDraft"
            };
            var processor = Factory.CreateDefaultProcessor<TestStrainerModule>();

            // Act
            var result = processor.Apply(model, _posts);

            // Assert
            result.Should().BeInDescendingOrder(p => p.IsDraft);
        }

        [Fact]
        public void IsSortableByMultipleProperties()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Sorts = "-IsDraft,-LikeCount"
            };
            var processor = Factory.CreateDefaultProcessor<TestStrainerModule>();

            // Act
            var result = processor.Apply(model, _posts);
            var sortedResult = _posts.OrderByDescending(p => p.IsDraft)
                .ThenByDescending(p => p.LikeCount);

            // Assert
            result.Should().BeInDescendingOrder(p => p.IsDraft);
            result.Should().ContainInOrder(sortedResult);
        }

        [Fact]
        public void IsFilterableNullableInts()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "CategoryId==1"
            };
            var processor = Factory.CreateDefaultProcessor<TestStrainerModule>();

            // Act
            var result = processor.Apply(model, _posts);

            // Assert
            result.Should().OnlyContain(p => p.CategoryId == 1);
        }
    }
}
