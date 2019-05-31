using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sieve.Exceptions;
using Sieve.Models;
using Sieve.Services;
using Sieve.Services.Filtering;
using Sieve.Services.Sorting;
using Sieve.UnitTests.Entities;
using Sieve.UnitTests.Services;

namespace Sieve.UnitTests
{
    [TestClass]
    public class Mapper
    {
        private readonly SieveContext _context;
        private readonly ApplicationSieveProcessor _processor;
        private readonly IQueryable<Post> _posts;

        public Mapper()
        {
            var options = new SieveOptionsAccessor();

            var filterOperatorProvider = new FilterOperatorProvider();
            var filterOperatorParser = new FilterOperatorParser(filterOperatorProvider);
            var filterOperatorValidator = new FilterOperatorValidator();
            var filterTermParser = new FilterTermParser(filterOperatorParser);
            var filteringContext = new FilteringContext(filterOperatorParser, filterOperatorProvider, filterOperatorValidator, filterTermParser);

            var sortTermParser = new SortTermParser();
            var sortingContext = new SortingContext(sortTermParser);

            var mapper = new SievePropertyMapper();

            var customFilterMethods = new SieveCustomFilterMethods();
            var customSortMethods = new SieveCustomSortMethods();
            var customMethodsContext = new SieveCustomMethodsContext(customFilterMethods, customSortMethods);

            _context = new SieveContext(
                options,
                filteringContext,
                sortingContext,
                mapper,
                customMethodsContext);

            _processor = new ApplicationSieveProcessor(_context);

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
            var model = new SieveModel()
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
            var model = new SieveModel()
            {
                Filters = "OnlySortableViaFluentApi@=50",
                Sorts = "OnlySortableViaFluentApi"
            };

            var result = _processor.Apply(model, _posts, applyFiltering: false, applyPagination: false);

            Assert.ThrowsException<SieveMethodNotFoundException>(() => _processor.Apply(model, _posts));

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
