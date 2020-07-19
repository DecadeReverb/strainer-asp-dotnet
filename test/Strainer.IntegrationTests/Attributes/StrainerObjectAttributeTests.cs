using FluentAssertions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;
using System;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Attributes
{
    public class StrainerObjectAttributeTests : StrainerFixtureBase
    {
        public StrainerObjectAttributeTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void StrainerObjectAttribute_With_TurnedOffFilters_DoesNotWork()
        {
            // Arrange
            var source = new[]
            {
                new EmptyTestEntity
                {
                    Name = "foo",
                },
                new EmptyTestEntity
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
                new SortableTestEntity
                {
                    Name = "foo",
                },
                new SortableTestEntity
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
                new FilterableTestEntity
                {
                    Name = "foo",
                },
                new FilterableTestEntity
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
                new FilterableAndSortableTestEntity
                {
                    Name = "fooZ",
                },
                new FilterableAndSortableTestEntity
                {
                    Name = "fooA",
                },
                new FilterableAndSortableTestEntity
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
                new DerivedTestEntity
                {
                    Name = "foo",
                },
                new DerivedTestEntity
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
        public void StrainerObjectAttribute_Filterable_Does_Not_Override_Property_Attributes()
        {
            // Arrange
            var source = new[]
            {
                new ObjectAndPropertyAttributesEntity
                {
                    Name = "foo",
                },
                new ObjectAndPropertyAttributesEntity
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

        [StrainerObject(nameof(Name), IsFilterable = false)]
        private class EmptyTestEntity
        {
            public string Name { get; set; }
        }

        private class DerivedTestEntity : SortableTestEntity
        {

        }

        [StrainerObject(nameof(Name), IsFilterable = false)]
        private class ObjectAndPropertyAttributesEntity
        {
            [StrainerProperty(IsFilterable = true)]
            public string Name { get; set; }
        }

        [StrainerObject(nameof(Name), IsFilterable = true)]
        private class FilterableTestEntity
        {
            public string Name { get; set; }
        }

        [StrainerObject(nameof(Name), IsSortable = true)]
        private class SortableTestEntity
        {
            public string Name { get; set; }
        }

        [StrainerObject(nameof(Name), IsFilterable = true, IsSortable = true)]
        private class FilterableAndSortableTestEntity
        {
            public string Name { get; set; }
        }
    }
}
