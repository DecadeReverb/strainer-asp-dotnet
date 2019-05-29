﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using Sieve.Models;
using Sieve.UnitTests.Entities;
using Sieve.UnitTests.Services;
using System.Linq;
using System.Collections.Generic;
using Sieve.Exceptions;
using Sieve.Services;

namespace Sieve.UnitTests
{
	[TestClass]
    public class Mapper
    {
        private readonly ApplicationSieveProcessor _processor;
        private readonly IQueryable<Post> _posts;

        public Mapper()
        {
            _processor = new ApplicationSieveProcessor(
                new SieveOptionsAccessor(),
                new FilterOperatorProvider(),
                new SieveCustomSortMethods(),
                new SieveCustomFilterMethods());

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
