using FluentAssertions;
using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.IntegrationTests.Services;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;
using Fluorite.Strainer.TestModels;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests
{
    public class General
    {
        private readonly StrainerContext _context;
        private readonly StrainerProcessor _processor;
        private readonly IQueryable<Post> _posts;
        private readonly IQueryable<Comment> _comments;

        public General()
        {
            var options = new StrainerOptionsAccessor();
            var mapper = new StrainerPropertyMapper(options);
            var metadataProvider = new StrainerPropertyMetadataProvider(mapper, options);

            var filterExpressionProvider = new FilterExpressionProvider();
            var filterOperatorValidator = new FilterOperatorValidator();
            var filterOperatorMapper = new FilterOperatorMapper(filterOperatorValidator);
            var filterOperatorParser = new FilterOperatorParser(filterOperatorMapper);
            var filterTermParser = new FilterTermParser(filterOperatorParser, filterOperatorMapper);
            var filteringContext = new FilteringContext(filterExpressionProvider, filterOperatorMapper, filterOperatorParser, filterOperatorValidator, filterTermParser);

            var sortExpressionProvider = new SortExpressionProvider(mapper, metadataProvider);
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
                metadataProvider,
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
                    Text = "This is a fairly new comment. text"
                },
                new Comment() {
                    Id = 2,
                    DateCreated = DateTimeOffset.UtcNow,
                    Text = "This is a brand new comment. (Text in braces)"
                },
            }.AsQueryable();
        }

        [Fact]
        public void ContainsCanBeCaseInsensitive()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "Title@=*a"
            };

            // Act
            var result = _processor.Apply(model, _posts);

            // Assert
            result.Should().OnlyContain(p => p.Title.Contains("a", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void ContainsIsCaseSensitive()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "Text@=text",
            };

            // Act
            var result = _processor.Apply(model, _comments);

            // Assert
            result.Should().OnlyContain(p => p.Text.Contains("text", StringComparison.Ordinal));
        }

        [Fact]
        public void NotContainsWorks()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "Title!@=D",
            };

            // Act
            var result = _processor.Apply(model, _posts);

            // Assert
            result.Should().OnlyContain(p => !p.Title.Contains("D", StringComparison.Ordinal));
        }

        [Fact]
        public void CanFilterBools()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "IsDraft==false"
            };

            // Act
            var result = _processor.Apply(model, _posts);

            // Assert
            result.Should().OnlyContain(p => !p.IsDraft);
        }

        [Fact]
        public void CanSortBools()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Sorts = "-IsDraft"
            };

            // Act
            var result = _processor.Apply(model, _posts);

            // Assert
            result.Should().BeInDescendingOrder(p => p.IsDraft);
        }

        [Fact]
        public void CanSortByMultipleProperties()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Sorts = "-IsDraft,-LikeCount"
            };

            // Act
            var result = _processor.Apply(model, _posts);
            var sortedResult = _posts.OrderByDescending(p => p.IsDraft)
                .ThenByDescending(p => p.LikeCount);

            // Assert
            result.Should().BeInDescendingOrder(p => p.IsDraft);
            result.Should().ContainInOrder(sortedResult);
        }

        [Fact]
        public void CanFilterNullableInts()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "CategoryId==1"
            };

            // Act
            var result = _processor.Apply(model, _posts);

            // Assert
            result.Should().OnlyContain(p => p.CategoryId == 1);
        }

        [Fact]
        public void EqualsDoesntFailWithNonStringTypes()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "LikeCount==50",
            };

            // Act
            var result = _processor.Apply(model, _posts);

            // Assert
            result.Should().OnlyContain(p => p.LikeCount == 50);
        }

        [Fact]
        public void CustomFiltersWork()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "Isnew",
            };

            // Act
            var result = _processor.Apply(model, _posts);

            // Assert
            result.Should().OnlyContain(p => p.LikeCount < 100);
        }

        [Fact]
        public void CustomFiltersWithOperatorsWork()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "HasInTitle==A",
            };

            // Act
            var result = _processor.Apply(model, _posts);

            // Assert
            result.Should().OnlyContain(p => p.Title.Contains("A"));
        }

        [Fact]
        public void CustomFiltersMixedWithUsualWork1()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "Isnew,CategoryId==2",
            };

            // Act
            var result = _processor.Apply(model, _posts);

            // Assert
            result.Should().OnlyContain(p => p.LikeCount < 100);
            result.Should().OnlyContain(p => p.CategoryId == 2);
        }

        [Fact]
        public void CustomFiltersMixedWithUsualWork2()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "CategoryId==2,Isnew",
            };

            // Act
            var result = _processor.Apply(model, _posts);

            // Assert
            result.Should().OnlyContain(p => p.CategoryId == 2);
            result.Should().OnlyContain(p => p.LikeCount < 100);
        }

        [Fact]
        public void CustomFiltersOnDifferentSourcesCanShareName()
        {
            // Arrange
            var postModel = new StrainerModel()
            {
                Filters = "CategoryId==2,Isnew",
            };

            // Act
            var postResult = _processor.Apply(postModel, _posts);

            // Assert
            postResult.Should().OnlyContain(p => p.CategoryId == 2);
            postResult.Should().OnlyContain(p => p.LikeCount < 100);

            // ###

            // Arrange
            var commentModel = new StrainerModel()
            {
                Filters = "Isnew",
            };

            // Act
            var commentResult = _processor.Apply(commentModel, _comments);

            // Assert
            commentResult.Should().OnlyContain(c => c.DateCreated > DateTimeOffset.UtcNow.AddDays(-2));
        }

        [Fact]
        public void CustomSortsWork()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Sorts = "Popularity",
            };

            // Act
            var result = _processor.Apply(model, _posts);
            var customSortResult = _posts.OrderBy(p => p.LikeCount)
                    .ThenBy(p => p.CommentCount)
                    .ThenBy(p => p.DateCreated);

            // Assert
            result.Should().HaveSameCount(_posts);
            result.Should().BeInAscendingOrder(p => p.LikeCount)
                .And.ContainInOrder(customSortResult);
        }

        [Fact]
        public void MethodNotFoundExceptionWork()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "does not exist",
            };

            // Assert
            Assert.Throws<StrainerMethodNotFoundException>(() => _processor.Apply(model, _posts));
        }

        [Fact]
        public void OrNameFilteringWorks()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "(Title|LikeCount)==3",
            };

            // Act
            var result = _processor.Apply(model, _posts);

            // Assert
            result.Should().OnlyContain(p => p.Title == "3" || p.LikeCount == 3);
        }

        [Fact]
        public void OrValueFilteringWorks()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "Title==C|D",
            };

            // Act
            var result = _processor.Apply(model, _posts);

            // Assert
            result.Should().OnlyContain(p => p.Title == "C" || p.Title == "D");
        }

        [Fact]
        public void OrValueFilteringWorks2()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "Text@=(|)",
            };

            // Act
            var result = _processor.Apply(model, _comments);

            // Assert
            Assert.Equal(1, result.Count());
            Assert.Equal(2, result.FirstOrDefault().Id);
        }

        [Fact]
        public void NestedFilteringWorks()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "TopComment.Text!@=A",
            };

            // Act
            var result = _processor.Apply(model, _posts);

            // Assert
            result.Should().NotContain(p => p.TopComment.Text.Contains("A"));
        }

        [Fact]
        public void NestedSortingWorks()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Sorts = "TopComment.Id",
            };

            // Act
            var result = _processor.Apply(model, _posts);

            // Assert
            result.Should().BeInAscendingOrder(post => post.TopComment.Id);
        }

        [Fact]
        public void NestedFilteringWithIdenticTypesWorks()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "(topc|featc)@=*B",
            };

            // Act
            var result = _processor.Apply(model, _posts);

            // Assert
            result.Should().OnlyContain(p => p.TopComment.Text.Contains("B") || p.FeaturedComment.Text.Contains("B"));
        }
    }
}
