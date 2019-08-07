using FluentAssertions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models;
using System;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Attributes.StrainerObject
{
    public class StrainerObjectAttributeTests : StrainerFixtureBase
    {
        public StrainerObjectAttributeTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void StrainerObjectAttribute_Empty_DoesNotWork()
        {
            // Arrange
            var source = new[]
            {
                new _EmptyTestEntity
                {
                    Name = "foo",
                },
                new _EmptyTestEntity
                {
                    Name = "bar",
                },
            }.AsQueryable();
            var processor = Factory.CreateDefaultProcessor();
            var model = new StrainerModel
            {
                Filters = "Name==foo",
                Sorts = "Name",
            };

            // Act
            var result = processor.Apply(model, source);

            // Assert
            result.Should().BeEquivalentTo(source);
        }

        [Fact]
        public void StrainerObjectAttribute_Sortable_Works()
        {
            // Arrange
            var source = new[]
            {
                new _SortableTestEntity
                {
                    Name = "foo",
                },
                new _SortableTestEntity
                {
                    Name = "bar",
                },
            }.AsQueryable();
            var processor = Factory.CreateDefaultProcessor();
            var model = new StrainerModel
            {
                Sorts = "Name",
            };

            // Act
            var result = processor.ApplySorting(model, source);

            // Assert
            result.Should().BeInAscendingOrder(e => e.Name);
        }

        [Fact]
        public void StrainerObjectAttribute_Filterable_Works()
        {
            // Arrange
            var source = new[]
            {
                new _FilterableTestEntity
                {
                    Name = "foo",
                },
                new _FilterableTestEntity
                {
                    Name = "bar",
                },
            }.AsQueryable();
            var processor = Factory.CreateDefaultProcessor();
            var model = new StrainerModel
            {
                Filters = "Name==foo",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(e => e.Name == "foo");
        }

        [Fact]
        public void StrainerObjectAttribute_FilterableAndSortable_Works()
        {
            // Arrange
            var source = new[]
            {
                new _FilterableAndSortableTestEntity
                {
                    Name = "fooZ",
                },
                new _FilterableAndSortableTestEntity
                {
                    Name = "fooA",
                },
                new _FilterableAndSortableTestEntity
                {
                    Name = "bar",
                },
            }.AsQueryable();
            var processor = Factory.CreateDefaultProcessor();
            var model = new StrainerModel
            {
                Filters = "Name@=foo",
                Sorts = "Name",
            };

            // Act
            var result = processor.Apply(model, source);

            // Assert
            result.Should().OnlyContain(e => e.Name.Contains("foo", StringComparison.OrdinalIgnoreCase));
            result.Should().BeInAscendingOrder(e => e.Name);
        }

        [Fact]
        public void StrainerObjectAttribute_Sortable_Works_For_BaseProperties()
        {
            // Arrange
            var source = new[]
            {
                new _DerivedTestEntity
                {
                    Name = "foo",
                },
                new _DerivedTestEntity
                {
                    Name = "bar",
                },
            }.AsQueryable();
            var processor = Factory.CreateDefaultProcessor();
            var model = new StrainerModel
            {
                Sorts = "Name",
            };

            // Act
            var result = processor.ApplySorting(model, source);

            // Assert
            result.Should().BeInAscendingOrder(e => e.Name);
        }
    }

    [StrainerObject]
    class _EmptyTestEntity
    {
        public string Name { get; set; }
    }

    class _DerivedTestEntity : _SortableTestEntity
    {

    }

    [StrainerObject(IsFilterable = true)]
    class _FilterableTestEntity
    {
        public string Name { get; set; }
    }

    [StrainerObject(IsSortable = true)]
    class _SortableTestEntity
    {
        public string Name { get; set; }
    }

    [StrainerObject(IsFilterable = true, IsSortable = true)]
    class _FilterableAndSortableTestEntity
    {
        public string Name { get; set; }
    }
}
