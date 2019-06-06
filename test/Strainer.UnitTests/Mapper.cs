using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;
using Fluorite.Strainer.UnitTests.Entities;
using Fluorite.Strainer.UnitTests.Services;

namespace Fluorite.Strainer.UnitTests
{
    [TestClass]
    public class Mapper
    {
        private readonly StrainerContext _context;
        private readonly ApplicationStrainerProcessor _processor;
        private readonly IQueryable<Post> _posts;

        public Mapper()
        {
            var options = new StrainerOptionsAccessor();

            var filterOperatorProvider = new FilterOperatorProvider();
            var filterOperatorParser = new FilterOperatorParser(filterOperatorProvider);
            var filterOperatorValidator = new FilterOperatorValidator();
            var filterTermParser = new FilterTermParser(filterOperatorParser);
            var filteringContext = new FilteringContext(filterOperatorParser, filterOperatorProvider, filterOperatorValidator, filterTermParser);

            var sortingWayFormatter = new SortingWayFormatter();
            var sortTermParser = new SortTermParser(sortingWayFormatter);
            var sortingContext = new SortingContext(sortingWayFormatter, sortTermParser);

            var mapper = new StrainerPropertyMapper();

            var customFilterMethods = new StrainerCustomFilterMethods();
            var customSortMethods = new StrainerCustomSortMethods();
            var customMethodsContext = new StrainerCustomMethodsContext(customFilterMethods, customSortMethods);

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

        [TestMethod]
        public void MapperWorks()
        {
            var model = new StrainerModel()
            {
                Filters = "shortname@=A",
            };

            var result = _processor.Apply(model, _posts);

            Assert.AreEqual(result.First().ThisHasNoAttributeButIsAccessible, "A");

            Assert.IsTrue(result.Count() == 1);
        }

        [TestMethod]
        public void MapperSortOnlyWorks()
        {
            var model = new StrainerModel()
            {
                Filters = "OnlySortableViaFluentApi@=50",
                Sorts = "OnlySortableViaFluentApi"
            };

            var result = _processor.Apply(model, _posts, applyFiltering: false, applyPagination: false);

            Assert.ThrowsException<StrainerMethodNotFoundException>(() => _processor.Apply(model, _posts));

            Assert.AreEqual(result.First().Id, 3);

            Assert.IsTrue(result.Count() == 3);
        }
    }
}

//
//Sorts = "LikeCount",
//Page = 1,
//PageSize = 10
//
