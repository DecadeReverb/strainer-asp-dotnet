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
                    Factory.CreateOptionsProvider(),
                    context.Filter,
                    newSortingContext,
                    context.Mapper,
                    context.MetadataProvider,
                    context.CustomMethods);

                return new _TestStrainerProcessor(newContext);
            });
            var model = new StrainerModel
            {
                Sorts = "Name" + _TestSortingWayFormatter.AscendingSuffix
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
                .IsSortable()
                .IsDefaultSort();
        }
    }

    class _TestEntity
    {
        public string Name { get; set; }
    }

    class _TestSortingWayFormatter : ISortingWayFormatter
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

            return input + GetSuffix(isDescending);
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

        private string GetSuffix(bool isDescending)
        {
            return isDescending
                ? DescendingSuffix
                : AscendingSuffix;
        }
    }
}
