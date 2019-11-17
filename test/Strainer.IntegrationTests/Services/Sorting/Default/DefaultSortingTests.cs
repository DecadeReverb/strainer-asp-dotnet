using FluentAssertions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Services.Sorting.Default
{
    public class DefaultSortingTests : StrainerFixtureBase
    {
        public DefaultSortingTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void DefaultSorting_Works_WithEmptyModel()
        {
            // Arrange
            var source = new[]
            {
                new Post
                {
                    Name = "Foo",
                },
                new Post
                {
                    Name = "Bar",
                },
            }.AsQueryable();
            var processor = Factory.CreateDefaultProcessor();
            var model = new StrainerModel();

            // Act
            var result = processor.ApplySorting(model, source);

            // Assert
            result.Should().BeInAscendingOrder(e => e.Name);
        }

        [Fact]
        public void DefaultSorting_Works()
        {
            // Arrange
            var source = new[]
            {
                new Post
                {
                    Title = "Foo",
                },
                new Post
                {
                    Title = "Bar",
                },
            }.AsQueryable();
            var processor = Factory.CreateDefaultProcessor(options =>
            {
                options.ThrowExceptions = false;
            });
            var model = new StrainerModel();

            // Act
            var result = processor.ApplySorting(model, source);

            // Assert
            result.Should().BeInAscendingOrder(e => e.Name);
        }

        [Fact]
        public void DefaultSorting_DoesNotWork_When_NoDefaultSortingIsDefined()
        {
            // Arrange
            var source = new[]
            {
                new Post
                {
                    Title = "Foo",
                },
                new Post
                {
                    Title = "Bar",
                },
            }.AsQueryable();
            var processor = Factory.CreateDefaultProcessor();
            var model = new StrainerModel();

            // Act
            var result = processor.ApplySorting(model, source);

            // Assert
            result.Should().BeEquivalentTo(source);
            result.Should().BeInDescendingOrder(e => e.Name);
        }

        private class Post
        {
            [StrainerProperty(IsSortable = true, IsDefaultSorting = true)]
            public string Name { get; set; }

            public string Title { get; set; }
        }
    }
}
