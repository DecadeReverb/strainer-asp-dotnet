using FluentAssertions;
using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Modules;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Sorting
{
    public class CustomSortingTests : StrainerFixtureBase
    {
        public CustomSortingTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void CustomSortsWork()
        {
            // Arrange
            var queryable = new List<Post>
            {
                new Post
                {
                    LikeCount = 0,
                    CommentCount = 0,
                    DateCreated = DateTime.UtcNow.AddDays(-2),
                },
                new Post
                {
                    LikeCount = 2,
                    CommentCount = 0,
                    DateCreated = DateTime.UtcNow.AddDays(-2),
                },
                new Post
                {
                    LikeCount = 0,
                    CommentCount = 2,
                    DateCreated = DateTime.UtcNow,
                },
            }.AsQueryable();
            var model = new StrainerModel()
            {
                Sorts = "Popularity",
            };
            var processor = Factory.CreateDefaultProcessor<TestStrainerModule>();

            // Act
            var result = processor.Apply(model, queryable);
            var customSortResult = queryable
                .OrderByDescending(p => p.LikeCount)
                .ThenByDescending(p => p.CommentCount)
                .ThenByDescending(p => p.DateCreated);

            // Assert
            result.Should().HaveSameCount(queryable);
            result.Should().ContainInOrder(customSortResult);
        }

        private class Post
        {
            public int CommentCount { get; set; }

            public DateTime DateCreated { get; set; }

            public int LikeCount { get; set; }
        }

        private class TestStrainerModule : StrainerModule<Post>
        {
            public override void Load(IStrainerModuleBuilder<Post> builder)
            {
                builder
                    .AddCustomSortMethod(nameof(Popularity))
                    .HasFunction(Popularity);
            }

            private IOrderedQueryable<Post> Popularity(IQueryable<Post> source, bool isDescending, bool isSubsequent)
            {
                return isSubsequent
                    ? (source as IOrderedQueryable<Post>)
                        .ThenByDescending(p => p.LikeCount)
                        .ThenByDescending(p => p.CommentCount)
                        .ThenByDescending(p => p.DateCreated)
                    : source
                        .OrderByDescending(p => p.LikeCount)
                        .ThenByDescending(p => p.CommentCount)
                        .ThenByDescending(p => p.DateCreated);
            }
        }
    }
}
