﻿using System;
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
    public class General
    {
        private readonly StrainerContext _context;
        private readonly StrainerProcessor _processor;
        private readonly IQueryable<Post> _posts;
        private readonly IQueryable<Comment> _comments;

        public General()
        {
            var options = new StrainerOptionsAccessor();

            var filterOperatorProvider = new FilterOperatorProvider();
            var filterOperatorParser = new FilterOperatorParser(filterOperatorProvider);
            var filterOperatorValidator = new FilterOperatorValidator();
            var filterTermParser = new FilterTermParser(filterOperatorParser);
            var filteringContext = new FilteringContext(filterOperatorParser, filterOperatorProvider, filterOperatorValidator, filterTermParser);

            var sortTermParser = new SortTermParser();
            var sortingContext = new SortingContext(sortTermParser);

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
                    Id = 0,
                    Title = "A",
                    LikeCount = 100,
                    IsDraft = true,
                    CategoryId = null,
                    TopComment = new Comment { Id = 0, Text = "A1" },
                    FeaturedComment = new Comment { Id = 4, Text = "A2" }
                },
                new Post() {
                    Id = 1,
                    Title = "B",
                    LikeCount = 50,
                    IsDraft = false,
                    CategoryId = 1,
                    TopComment = new Comment { Id = 3, Text = "B1" },
                    FeaturedComment = new Comment { Id = 5, Text = "B2" }
                },
                new Post() {
                    Id = 2,
                    Title = "C",
                    LikeCount = 0,
                    CategoryId = 1,
                    TopComment = new Comment { Id = 2, Text = "C1" },
                    FeaturedComment = new Comment { Id = 6, Text = "C2" }
                },
                new Post() {
                    Id = 3,
                    Title = "D",
                    LikeCount = 3,
                    IsDraft = true,
                    CategoryId = 2,
                    TopComment = new Comment { Id = 1, Text = "D1" },
                    FeaturedComment = new Comment { Id = 7, Text = "D2" }
                },
            }.AsQueryable();

            _comments = new List<Comment>
            {
                new Comment() {
                    Id = 0,
                    DateCreated = DateTimeOffset.UtcNow.AddDays(-20),
                    Text = "This is an old comment."
                },
                new Comment() {
                    Id = 1,
                    DateCreated = DateTimeOffset.UtcNow.AddDays(-1),
                    Text = "This is a fairly new comment."
                },
                new Comment() {
                    Id = 2,
                    DateCreated = DateTimeOffset.UtcNow,
                    Text = "This is a brand new comment. (Text in braces)"
                },
            }.AsQueryable();
        }

        [TestMethod]
        public void ContainsCanBeCaseInsensitive()
        {
            var model = new StrainerModel()
            {
                Filters = "Title@=*a"
            };

            var result = _processor.Apply(model, _posts);

            Assert.AreEqual(result.First().Id, 0);
            Assert.IsTrue(result.Count() == 1);
        }

        [TestMethod]
        public void ContainsIsCaseSensitive()
        {
            var model = new StrainerModel()
            {
                Filters = "Title@=a",
            };

            var result = _processor.Apply(model, _posts);

            Assert.IsTrue(result.Count() == 0);
        }

        [TestMethod]
        public void NotContainsWorks()
        {
            var model = new StrainerModel()
            {
                Filters = "Title!@=D",
            };

            var result = _processor.Apply(model, _posts);

            Assert.IsTrue(result.Count() == 3);
        }

        [TestMethod]
        public void CanFilterBools()
        {
            var model = new StrainerModel()
            {
                Filters = "IsDraft==false"
            };

            var result = _processor.Apply(model, _posts);

            Assert.IsTrue(result.Count() == 2);
        }

        [TestMethod]
        public void CanSortBools()
        {
            var model = new StrainerModel()
            {
                Sorts = "-IsDraft"
            };

            var result = _processor.Apply(model, _posts);

            Assert.AreEqual(result.First().Id, 0);
        }

        [TestMethod]
        public void CanFilterNullableInts()
        {
            var model = new StrainerModel()
            {
                Filters = "CategoryId==1"
            };

            var result = _processor.Apply(model, _posts);

            Assert.IsTrue(result.Count() == 2);
        }

        [TestMethod]
        public void EqualsDoesntFailWithNonStringTypes()
        {
            var model = new StrainerModel()
            {
                Filters = "LikeCount==50",
            };

            var parsedFilters = _context.FilteringContext.TermParser.GetParsedTerms(model.Filters);
            Console.WriteLine(parsedFilters.First().Values);
            Console.WriteLine(parsedFilters.First().Operator);
            Console.WriteLine(parsedFilters.First().OperatorParsed);

            var result = _processor.Apply(model, _posts);

            Assert.AreEqual(result.First().Id, 1);
            Assert.IsTrue(result.Count() == 1);
        }

        [TestMethod]
        public void CustomFiltersWork()
        {
            var model = new StrainerModel()
            {
                Filters = "Isnew",
            };

            var result = _processor.Apply(model, _posts);

            Assert.IsFalse(result.Any(p => p.Id == 0));
            Assert.IsTrue(result.Count() == 3);
        }

        [TestMethod]
        public void CustomFiltersWithOperatorsWork()
        {
            var model = new StrainerModel()
            {
                Filters = "HasInTitle==A",
            };

            var result = _processor.Apply(model, _posts);

            Assert.IsTrue(result.Any(p => p.Id == 0));
            Assert.IsTrue(result.Count() == 1);
        }

        [TestMethod]
        public void CustomFiltersMixedWithUsualWork1()
        {
            var model = new StrainerModel()
            {
                Filters = "Isnew,CategoryId==2",
            };

            var result = _processor.Apply(model, _posts);

            Assert.IsTrue(result.Any(p => p.Id == 3));
            Assert.IsTrue(result.Count() == 1);
        }

        [TestMethod]
        public void CustomFiltersMixedWithUsualWork2()
        {
            var model = new StrainerModel()
            {
                Filters = "CategoryId==2,Isnew",
            };

            var result = _processor.Apply(model, _posts);

            Assert.IsTrue(result.Any(p => p.Id == 3));
            Assert.IsTrue(result.Count() == 1);
        }

        [TestMethod]
        public void CustomFiltersOnDifferentSourcesCanShareName()
        {
            var postModel = new StrainerModel()
            {
                Filters = "CategoryId==2,Isnew",
            };

            var postResult = _processor.Apply(postModel, _posts);

            Assert.IsTrue(postResult.Any(p => p.Id == 3));
            Assert.AreEqual(1, postResult.Count());

            var commentModel = new StrainerModel()
            {
                Filters = "Isnew",
            };

            var commentResult = _processor.Apply(commentModel, _comments);

            Assert.IsTrue(commentResult.Any(c => c.Id == 2));
            Assert.AreEqual(2, commentResult.Count());
        }

        [TestMethod]
        public void CustomSortsWork()
        {
            var model = new StrainerModel()
            {
                Sorts = "Popularity",
            };

            var result = _processor.Apply(model, _posts);

            Assert.IsFalse(result.First().Id == 0);
        }

        [TestMethod]
        public void MethodNotFoundExceptionWork()
        {
            var model = new StrainerModel()
            {
                Filters = "does not exist",
            };

            Assert.ThrowsException<StrainerMethodNotFoundException>(() => _processor.Apply(model, _posts));
        }

        [TestMethod]
        public void IncompatibleMethodExceptionsWork()
        {
            var model = new StrainerModel()
            {
                Filters = "TestComment",
            };

            Assert.ThrowsException<StrainerIncompatibleMethodException>(() => _processor.Apply(model, _posts));
        }

        [TestMethod]
        public void OrNameFilteringWorks()
        {
            var model = new StrainerModel()
            {
                Filters = "(Title|LikeCount)==3",
            };

            var result = _processor.Apply(model, _posts);
            var entry = result.FirstOrDefault();
            var resultCount = result.Count();

            Assert.IsNotNull(entry);
            Assert.AreEqual(1, resultCount);
            Assert.AreEqual(3, entry.Id);
        }

        [TestMethod]
        public void OrValueFilteringWorks()
        {
            var model = new StrainerModel()
            {
                Filters = "Title==C|D",
            };

            var result = _processor.Apply(model, _posts);
            Assert.AreEqual(2, result.Count());
            Assert.IsTrue(result.Any(p => p.Id == 2));
            Assert.IsTrue(result.Any(p => p.Id == 3));
        }

        [TestMethod]
        public void OrValueFilteringWorks2()
        {
            var model = new StrainerModel()
            {
                Filters = "Text@=(|)",
            };

            var result = _processor.Apply(model, _comments);
            Assert.AreEqual(1, result.Count());
            Assert.AreEqual(2, result.FirstOrDefault().Id);
        }

        [TestMethod]
        public void NestedFilteringWorks()
        {
            var model = new StrainerModel()
            {
                Filters = "TopComment.Text!@=A",
            };

            var result = _processor.Apply(model, _posts);
            Assert.AreEqual(3, result.Count());
            var posts = result.ToList();
            Assert.IsTrue(posts[0].TopComment.Text.Contains("B"));
            Assert.IsTrue(posts[1].TopComment.Text.Contains("C"));
            Assert.IsTrue(posts[2].TopComment.Text.Contains("D"));
        }

        [TestMethod]
        public void NestedSortingWorks()
        {
            var model = new StrainerModel()
            {
                Sorts = "TopComment.Id",
            };

            var result = _processor.Apply(model, _posts);
            Assert.AreEqual(4, result.Count());
            var posts = result.ToList();
            Assert.AreEqual(posts[0].Id, 0);
            Assert.AreEqual(posts[1].Id, 3);
            Assert.AreEqual(posts[2].Id, 2);
            Assert.AreEqual(posts[3].Id, 1);
        }

        [TestMethod]
        public void NestedFilteringWithIdenticTypesWorks()
        {
            var model = new StrainerModel()
            {
                Filters = "(topc|featc)@=*2",
            };

            var result = _processor.Apply(model, _posts);
            Assert.AreEqual(4, result.Count());

            model = new StrainerModel()
            {
                Filters = "(topc|featc)@=*B",
            };

            result = _processor.Apply(model, _posts);
            Assert.AreEqual(1, result.Count());
        }
    }
}