﻿using FluentAssertions;
using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;
using Fluorite.Strainer.UnitTests.Entities;
using Fluorite.Strainer.UnitTests.Services;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.UnitTests
{
    public class Mapper
    {
        private readonly StrainerContext _context;
        private readonly ApplicationStrainerProcessor _processor;
        private readonly IQueryable<Post> _posts;

        public Mapper()
        {
            var options = new StrainerOptionsAccessor();
            var mapper = new StrainerPropertyMapper();

            var filterOperatorValidator = new FilterOperatorValidator();
            var filterOperatorMapper = new FilterOperatorMapper(filterOperatorValidator);
            var filterOperatorParser = new FilterOperatorParser(filterOperatorMapper);
            var filterTermParser = new FilterTermParser(filterOperatorParser, filterOperatorMapper);
            var filteringContext = new FilteringContext(filterOperatorMapper, filterOperatorParser, filterOperatorValidator, filterTermParser);

            var sortExpressionProvider = new SortExpressionProvider(mapper, options);
            var sortingWayFormatter = new SortingWayFormatter();
            var sortTermParser = new SortTermParser(sortingWayFormatter);
            var sortingContext = new SortingContext(sortExpressionProvider, sortingWayFormatter, sortTermParser);

            var customFilterMethodMapper = new CustomFilterMethodMapper(options);
            var customFilterMethodProvider = new ApplicationCustomFilterMethodProvider(customFilterMethodMapper);

            var customSortMethodMapper = new CustomSortMethodMapper(options);
            var customSortMethodProvider = new ApplicationCustomSortMethodProvider(customSortMethodMapper);

            var customMethodsContext = new CustomMethodsContext(customFilterMethodProvider, customSortMethodProvider);

            _context = new StrainerContext(
                options,
                filteringContext,
                sortingContext,
                mapper,
                customMethodsContext);

            _processor = new ApplicationStrainerProcessor(_context);

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

            // Act
            var result = _processor.Apply(model, _posts);

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

            // Act
            var result = _processor.Apply(model, _posts, applyFiltering: false, applyPagination: false);

            // Assert
            Assert.Throws<StrainerMethodNotFoundException>(() => _processor.Apply(model, _posts));
            result.Should().BeInAscendingOrder(post => post.OnlySortableViaFluentApi);
            result.Should().HaveCount(3);
        }
    }
}
