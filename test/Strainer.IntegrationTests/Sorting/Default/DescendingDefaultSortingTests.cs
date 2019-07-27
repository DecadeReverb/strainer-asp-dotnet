using FluentAssertions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Sorting.Default
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
                new _TestBlog
                {
                    Name = "Foo",
                },
                new _TestBlog
                {
                    Name = "Bar",
                },
            }.AsQueryable();
            var processor = Factory.CreateProcessor(context => new _TestDescendingStrainerProcessor(context));
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
                new _TestBlog
                {
                    Name = "Foo",
                },
                new _TestBlog
                {
                    Name = "Bar",
                },
            }.AsQueryable();
            var processor = Factory.CreateProcessor(context =>
            {
                context.Options.ThrowExceptions = false;

                return new _TestDescendingStrainerProcessor(context);
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
    }

    class _TestDescendingStrainerProcessor : StrainerProcessor
    {
        public _TestDescendingStrainerProcessor(IStrainerContext context) : base(context)
        {

        }

        protected override void MapProperties(IPropertyMapper mapper)
        {
            mapper.Property<_TestBlog>(e => e.Name)
                .IsSortable()
                .IsDefaultSort(isDescending: true);
        }
    }

    class _TestBlog
    {
        public string Name { get; set; }
    }
}
