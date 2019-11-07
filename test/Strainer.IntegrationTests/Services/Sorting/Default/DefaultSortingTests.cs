using FluentAssertions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
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
            var processor = Factory.CreateProcessor(context => new TestStrainerProcessor(context));
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
                    Name = "Foo",
                },
                new Post
                {
                    Name = "Bar",
                },
            }.AsQueryable();
            var processor = Factory.CreateProcessor(context =>
            {
                context.Options.ThrowExceptions = false;

                return new TestStrainerProcessor(context);
            });
            var model = new StrainerModel
            {
                Sorts = "Title",
            };

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
            result.Should().BeEquivalentTo(source);
            result.Should().BeInDescendingOrder(e => e.Name);
        }

        private class TestStrainerProcessor : StrainerProcessor
        {
            public TestStrainerProcessor(IStrainerContext context) : base(context)
            {

            }

            protected override void MapProperties(IPropertyMapper mapper)
            {
                mapper.Property<Post>(p => p.Name)
                    .IsSortable()
                    .IsDefaultSort();
            }
        }

        private class Post
        {
            public string Name { get; set; }
        }
    }
}
