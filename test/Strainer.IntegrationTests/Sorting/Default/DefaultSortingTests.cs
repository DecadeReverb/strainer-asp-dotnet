using FluentAssertions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Sorting.Default
{
    public class DefaultSortingTests : StrainerFixtureBase
    {
        public DefaultSortingTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void DefaultSorting_Works()
        {
            // Arrange
            var source = new[]
            {
                new _TestEntity
                {
                    Name = "Foo",
                },
                new _TestEntity
                {
                    Name = "Bar",
                },
            }.AsQueryable();
            var processor = Factory.CreateProcessor(context => new _TestStrainerProcessor(context));
            var model = new StrainerModel();

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

        protected override void MapProperties(IStrainerPropertyMapper mapper)
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
}
