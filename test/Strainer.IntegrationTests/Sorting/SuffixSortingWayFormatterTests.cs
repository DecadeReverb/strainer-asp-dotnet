using FluentAssertions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Sorting;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Sorting
{
    public class SuffixSortingWayFormatterTests : StrainerFixtureBase
    {
        public SuffixSortingWayFormatterTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void CustomSortingWayFormatter_Works_ForAscendingSorting()
        {
            // Arrange
            var source = new[]
            {
                new Post
                {
                    Name = "foo",
                },
                new Post
                {
                    Name = "bar",
                },
            }.AsQueryable();
            var processor = Factory.CreateProcessorWithSortingWayFormatter<SuffixSortingWayFormatter>();
            var model = new StrainerModel
            {
                Sorts = $"{nameof(Post.Name)}{SuffixSortingWayFormatter.AscendingSuffix}",
            };

            // Act
            var result = processor.ApplySorting(model, source);

            // Assert
            result.Should().BeInAscendingOrder(e => e.Name);
        }

        [Fact]
        public void CustomSortingWayFormatter_Works_ForDescendingSorting()
        {
            // Arrange
            var source = new[]
            {
                new Post
                {
                    Name = "bar",
                },
                new Post
                {
                    Name = "foo",
                },
            }.AsQueryable();
            var processor = Factory.CreateProcessorWithSortingWayFormatter<SuffixSortingWayFormatter>();
            var model = new StrainerModel
            {
                Sorts = $"{nameof(Post.Name)}{SuffixSortingWayFormatter.DescendingSuffix}",
            };

            // Act
            var result = processor.ApplySorting(model, source);

            // Assert
            result.Should().BeInDescendingOrder(e => e.Name);
        }

        private class Post
        {
            [StrainerProperty(IsDefaultSorting = true)]
            public string Name { get; set; }
        }
    }
}
