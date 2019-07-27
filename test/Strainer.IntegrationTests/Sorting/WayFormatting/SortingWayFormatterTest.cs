using FluentAssertions;
using Fluorite.Extensions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Sorting;
using System;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Sorting.WayFormatting
{
    public class SortingWayFormatterTest : StrainerFixtureBase
    {
        public SortingWayFormatterTest(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void CustomSortingWayFormatterWorks()
        {
            // Arrange
            var source = new[]
            {
                new _TestEntity
                {
                    Name = "foo",
                },
                new _TestEntity
                {
                    Name = "bar",
                },
            }.AsQueryable();
            var customSortingWayFormatter = new _TestSortingWayFormatter();
            var processor = Factory.CreateProcessor(context =>
            {
                var newSortingContext = new SortingContext(
                    context.Sorting.ExpressionProvider,
                    context.Sorting.ExpressionValidator,
                    customSortingWayFormatter,
                    context.Sorting.TermParser);
                var newContext = new StrainerContext(
                    context.Options,
                    context.Filter,
                    newSortingContext,
                    context.Mapper,
                    context.MetadataProvider,
                    context.CustomMethods);

                return new _TestStrainerProcessor(newContext);
            });
            var model = new StrainerModel
            {
                Sorts = "Name" + _TestSortingWayFormatter.AscendingSortingWaySuffix
            };

            // Act
            var result = processor.ApplySorting(model, source);

            // Assert
            result.Should().BeInAscendingOrder(e => e.Name);
        }
    }

    class _TestStrainerProcessor : StrainerProcessor
    {
        public _TestStrainerProcessor(IStrainerContext context) : base(context)
        {

        }

        protected override void MapProperties(IPropertyMapper mapper)
        {
            mapper.Property<_TestEntity>(e => e.Name)
                .CanSort()
                .IsDefaultSort();
        }
    }

    class _TestEntity
    {
        public string Name { get; set; }
    }

    class _TestSortingWayFormatter : ISortingWayFormatter
    {
        public static readonly string AscendingSortingWaySuffix = "_asc";
        public static readonly string DescendingSortingWaySuffix = "_desc";

        public string Format(string input, bool isDescending)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            var sortingWaySuffix = isDescending ? DescendingSortingWaySuffix : AscendingSortingWaySuffix;

            return input + sortingWaySuffix;
        }

        public bool IsDescending(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            return input.EndsWith(DescendingSortingWaySuffix);
        }

        public string Unformat(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }

            if (input.EndsWith(AscendingSortingWaySuffix))
            {
                return input.TrimEndOnce(AscendingSortingWaySuffix);
            }
            else
            {
                if (input.EndsWith(DescendingSortingWaySuffix))
                {
                    return input.TrimEndOnce(DescendingSortingWaySuffix);
                }
                else
                {
                    return input;
                }
            }
        }
    }
}
