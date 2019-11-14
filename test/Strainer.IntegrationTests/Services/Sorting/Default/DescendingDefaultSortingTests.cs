using FluentAssertions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Metadata;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Services.Sorting.Default
{
    public class DescendingDefaultSortingTests : StrainerFixtureBase
    {
        public DescendingDefaultSortingTests(StrainerFactory factory) : base(factory)
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
            var processor = Factory.CreateProcessor(context => new TestDescendingStrainerProcessor(context));
            var model = new StrainerModel();

            // Act
            var result = processor.ApplySorting(model, source);

            // Assert
            result.Should().BeInDescendingOrder(e => e.Name);
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

                return new TestDescendingStrainerProcessor(context);
            });
            var model = new StrainerModel
            {
                Sorts = "Title",
            };

            // Act
            var result = processor.ApplySorting(model, source);

            // Assert
            result.Should().BeInDescendingOrder(e => e.Name);
        }

        private class TestDescendingStrainerProcessor : StrainerProcessor
        {
            public TestDescendingStrainerProcessor(IStrainerContext context) : base(context)
            {

            }

            protected override void MapProperties(IPropertyMetadataMapper mapper)
            {
                mapper.Property<Post>(p => p.Name)
                    .IsSortable()
                    .IsDefaultSort(isDescending: true);
            }
        }

        private class Post
        {
            public string Name { get; set; }
        }
    }
}
