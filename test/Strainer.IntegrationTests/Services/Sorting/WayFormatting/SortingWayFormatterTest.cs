using FluentAssertions;
using Fluorite.Extensions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Sorting;
using System;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Services.Sorting.WayFormatting
{
    public class SortingWayFormatterTest : StrainerFixtureBase
    {
        public SortingWayFormatterTest(StrainerFactory factory) : base(factory)
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
            var customSortingWayFormatter = new TestSortingWayFormatter();
            var processor = Factory.CreateProcessor(context =>
            {
                var newSortingContext = new SortingContext(
                    context.Sorting.ExpressionProvider,
                    context.Sorting.ExpressionValidator,
                    customSortingWayFormatter,
                    context.Sorting.TermParser);
                var newContext = new StrainerContext(
                    Factory.CreateOptionsProvider(),
                    context.Filter,
                    newSortingContext,
                    context.Mapper,
                    context.AttributeMetadataProvider,
                    context.CustomMethods);

                return new TestStrainerProcessor(newContext);
            });
            var model = new StrainerModel
            {
                Sorts = "Name" + TestSortingWayFormatter.AscendingSuffix
            };

            // Act
            var result = processor.ApplySorting(model, source);

            // Assert
            result.Should().BeInAscendingOrder(e => e.Name);
        }

        private class TestStrainerProcessor : StrainerProcessor
        {
            public TestStrainerProcessor(IStrainerContext context) : base(context)
            {

            }

            protected override void MapProperties(IPropertyMapper mapper)
            {
                mapper.Property<Post>(e => e.Name)
                    .IsSortable()
                    .IsDefaultSort();
            }
        }

        private class Post
        {
            public string Name { get; set; }
        }

        private class TestSortingWayFormatter : ISortingWayFormatter
        {
            public static readonly string AscendingSuffix = "_asc";

            public static readonly string DescendingSuffix = "_desc";

            public bool IsDescendingDefaultSortingWay => true;

            public string Format(string input, bool isDescending)
            {
                if (input is null)
                {
                    throw new ArgumentNullException(nameof(input));
                }

                if (string.IsNullOrWhiteSpace(input))
                {
                    return input;
                }

                return input + (isDescending ? DescendingSuffix : AscendingSuffix);
            }

            public bool IsDescending(string input)
            {
                if (input is null)
                {
                    throw new ArgumentNullException(nameof(input));
                }

                if (input.EndsWith(DescendingSuffix))
                {
                    return true;
                }

                if (input.EndsWith(AscendingSuffix))
                {
                    return false;
                }

                return IsDescendingDefaultSortingWay;
            }

            public string Unformat(string input)
            {
                if (input is null)
                {
                    throw new ArgumentNullException(nameof(input));
                }

                if (string.IsNullOrEmpty(input))
                {
                    return input;
                }

                if (input.EndsWith(AscendingSuffix))
                {
                    return input.TrimEndOnce(AscendingSuffix);
                }
                else
                {
                    if (input.EndsWith(DescendingSuffix))
                    {
                        return input.TrimEndOnce(DescendingSuffix);
                    }

                    return input;
                }
            }
        }
    }
}
