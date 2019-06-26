using FluentAssertions;
using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.UnitTests.Entities;
using Fluorite.Strainer.UnitTests.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.UnitTests
{
    public class StrainerPropertyMapperTests : StrainerFixtureBase
    {
        private readonly IQueryable<Post> _posts;

        public StrainerPropertyMapperTests(StrainerFactory factory) : base(factory)
        {
            _posts = new List<Post>
            {
                new Post() {
                    Id = 1,
                    ThisHasNoAttributeButIsAccessible = "A",
                    ThisHasNoAttribute = "A",
                    OnlySortableViaFluentApi = 100
                },
                new Post() {
                    Id = 2,
                    ThisHasNoAttributeButIsAccessible = "B",
                    ThisHasNoAttribute = "B",
                    OnlySortableViaFluentApi = 50
                },
                new Post() {
                    Id = 3,
                    ThisHasNoAttributeButIsAccessible = "C",
                    ThisHasNoAttribute = "C",
                    OnlySortableViaFluentApi = 0
                },
            }.AsQueryable();
        }

        [Fact]
        public void MapperWorks()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "shortname@=A",
            };
            var processor = Factory.CreateProcessor(context => new ApplicationStrainerProcessor(context));

            // Act
            var result = processor.Apply(model, _posts);

            // Assert
            result.Should().NotBeEmpty();
            result.First().ThisHasNoAttribute.Should().Be("A");
        }

        [Fact]
        public void MapperSortOnlyWorks()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "OnlySortableViaFluentApi@=50",
                Sorts = "OnlySortableViaFluentApi"
            };
            var processor = Factory.CreateProcessor(context =>
            {
                context.Options.ThrowExceptions = true;

                return new ApplicationStrainerProcessor(context);
            });

            // Act
            var result = processor.Apply(model, _posts, applyFiltering: false, applyPagination: false);

            // Assert
            Assert.Throws<StrainerMethodNotFoundException>(() => processor.Apply(model, _posts));
            result.Should().BeInAscendingOrder(post => post.OnlySortableViaFluentApi);
            result.Should().HaveCount(3);
        }
    }
}
