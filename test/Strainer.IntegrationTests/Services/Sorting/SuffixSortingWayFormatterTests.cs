using FluentAssertions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Sorting;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Services.Sorting
{
    public class SuffixSortingWayFormatterTests : StrainerFixtureBase
    {
        public SuffixSortingWayFormatterTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void CustomSortingWayFormatter_Works()
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
            var customSortingWayFormatter = new SuffixSortingWayFormatter();
            var processor = Factory.CreateProcessor(context =>
            {
                var newSortingContext = new SortingContext(
                    context.Sorting.ExpressionProvider,
                    context.Sorting.ExpressionValidator,
                    customSortingWayFormatter,
                    new SortTermParser(
                        customSortingWayFormatter,
                        Factory.CreateOptionsProvider()));
                var newContext = new StrainerContext(
                    context.CustomMethods,
                    Factory.CreateOptionsProvider(),
                    context.Filter,
                    newSortingContext,
                    context.Metadata);

                return new StrainerProcessor(newContext);
            });
            var model = new StrainerModel
            {
                Sorts = "Name" + SuffixSortingWayFormatter.AscendingSuffix
            };

            // Act
            var result = processor.ApplySorting(model, source);

            // Assert
            result.Should().BeInAscendingOrder(e => e.Name);
        }

        private class Post
        {
            [StrainerProperty(IsSortable = true, IsDefaultSorting = true)]
            public string Name { get; set; }
        }
    }
}
