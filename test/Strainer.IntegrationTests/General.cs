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
    public class General : StrainerFixtureBase
    {
        private readonly IQueryable<Post> _posts;
        private readonly IQueryable<Comment> _comments;

        public General(StrainerFactory factory) : base(factory)
        {
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

            _posts = new List<Post>
            {
                new Post() {
                    Id = 0,
                    Title = "A",
                    LikeCount = 100,
                    IsDraft = true,
                    CategoryId = null,
                    TopComment = _comments.ElementAt(0),
                    FeaturedComment = _comments.ElementAt(0)
                },
                new Post() {
                    Id = 1,
                    Title = "B",
                    LikeCount = 50,
                    IsDraft = false,
                    CategoryId = 1,
                    TopComment = _comments.ElementAt(1),
                    FeaturedComment = _comments.ElementAt(1)
                },
                new Post() {
                    Id = 2,
                    Title = "C",
                    LikeCount = 0,
                    CategoryId = 1,
                    TopComment = _comments.ElementAt(2),
                    FeaturedComment = _comments.ElementAt(2)
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
        }

        [Fact]
        public void ContainsCanBeCaseInsensitive()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "Title@=*a"
            };
            var processor = Factory.CreateProcessor((context) => new ApplicationStrainerProcessor(context));

            // Act
            var result = processor.Apply(model, _posts);

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
            var processor = Factory.CreateProcessor((context) => new ApplicationStrainerProcessor(context));

            // Act
            var result = processor.Apply(model, _comments);

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
            var processor = Factory.CreateProcessor((context) => new ApplicationStrainerProcessor(context));

            // Act
            var result = processor.Apply(model, _posts);

            // Assert
            result.Should().OnlyContain(p => !p.Title.Contains("D", StringComparison.Ordinal));
        }

        [Fact]
        public void IsFilterableBools()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "IsDraft==false"
            };
            var processor = Factory.CreateProcessor((context) => new ApplicationStrainerProcessor(context));

            // Act
            var result = processor.Apply(model, _posts);

            // Assert
            result.Should().OnlyContain(p => !p.IsDraft);
        }

        [Fact]
        public void IsSortableBools()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Sorts = "-IsDraft"
            };
            var processor = Factory.CreateProcessor((context) => new ApplicationStrainerProcessor(context));

            // Act
            var result = processor.Apply(model, _posts);

            // Assert
            result.Should().BeInDescendingOrder(p => p.IsDraft);
        }

        [Fact]
        public void IsSortableByMultipleProperties()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Sorts = "-IsDraft,-LikeCount"
            };
            var processor = Factory.CreateProcessor((context) => new ApplicationStrainerProcessor(context));

            // Act
            var result = processor.Apply(model, _posts);
            var sortedResult = _posts.OrderByDescending(p => p.IsDraft)
                .ThenByDescending(p => p.LikeCount);

            // Assert
            result.Should().BeInDescendingOrder(p => p.IsDraft);
            result.Should().ContainInOrder(sortedResult);
        }

        [Fact]
        public void IsFilterableNullableInts()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "CategoryId==1"
            };
            var processor = Factory.CreateProcessor((context) => new ApplicationStrainerProcessor(context));

            // Act
            var result = processor.Apply(model, _posts);

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
            var processor = Factory.CreateProcessor((context) => new ApplicationStrainerProcessor(context));

            // Act
            var result = processor.Apply(model, _posts);

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
            var processor = Factory.CreateProcessor((context) =>
            {
                var options = new StrainerOptions();
                var customFilterMethodMapper = new CustomFilterMethodMapper(options);
                var customFilterMethodProvider = new CustomFilterMethodProvider(customFilterMethodMapper);
                var customMethodsContext = new CustomMethodsContext(options, customFilterMethodProvider);
                var newContext = new StrainerContext(
                    options,
                    context.Filter,
                    context.Sorting,
                    context.Mapper,
                    context.MetadataProvider,
                    customMethodsContext);

                return new ApplicationStrainerProcessor(newContext);
            });

            // Act
            var result = processor.Apply(model, _posts);

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
            var processor = Factory.CreateProcessor((context) =>
            {
                var options = new StrainerOptions();
                var customFilterMethodMapper = new CustomFilterMethodMapper(options);
                var customFilterMethodProvider = new CustomFilterMethodProvider(customFilterMethodMapper);
                var customMethodsContext = new CustomMethodsContext(options, customFilterMethodProvider);
                var newContext = new StrainerContext(
                    options,
                    context.Filter,
                    context.Sorting,
                    context.Mapper,
                    context.MetadataProvider,
                    customMethodsContext);

                return new ApplicationStrainerProcessor(newContext);
            });

            // Act
            var result = processor.Apply(model, _posts);

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
            var processor = Factory.CreateProcessor((context) =>
            {
                var options = new StrainerOptions();
                var customFilterMethodMapper = new CustomFilterMethodMapper(options);
                var customFilterMethodProvider = new CustomFilterMethodProvider(customFilterMethodMapper);
                var customMethodsContext = new CustomMethodsContext(options, customFilterMethodProvider);
                var newContext = new StrainerContext(
                    options,
                    context.Filter,
                    context.Sorting,
                    context.Mapper,
                    context.MetadataProvider,
                    customMethodsContext);

                return new ApplicationStrainerProcessor(newContext);
            });

            // Act
            var result = processor.Apply(model, _posts);

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
            var processor = Factory.CreateProcessor((context) =>
            {
                var options = new StrainerOptions();
                var customFilterMethodMapper = new CustomFilterMethodMapper(options);
                var customFilterMethodProvider = new CustomFilterMethodProvider(customFilterMethodMapper);
                var customMethodsContext = new CustomMethodsContext(options, customFilterMethodProvider);
                var newContext = new StrainerContext(
                    options,
                    context.Filter,
                    context.Sorting,
                    context.Mapper,
                    context.MetadataProvider,
                    customMethodsContext);

                return new ApplicationStrainerProcessor(newContext);
            });

            // Act
            var result = processor.Apply(model, _posts);

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
            var processor = Factory.CreateProcessor((context) =>
            {
                var options = new StrainerOptions();
                var customFilterMethodMapper = new CustomFilterMethodMapper(options);
                var customFilterMethodProvider = new CustomFilterMethodProvider(customFilterMethodMapper);
                var customMethodsContext = new CustomMethodsContext(options, customFilterMethodProvider);
                var newContext = new StrainerContext(
                    options,
                    context.Filter,
                    context.Sorting,
                    context.Mapper,
                    context.MetadataProvider,
                    customMethodsContext);

                return new ApplicationStrainerProcessor(newContext);
            });

            // Act
            var postResult = processor.Apply(postModel, _posts);

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
            var commentResult = processor.Apply(commentModel, _comments);

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
            var processor = Factory.CreateProcessor((context) =>
            {
                var options = new StrainerOptions();
                var customSortMethodMapper = new CustomSortMethodMapper(options);
                var customSortMethodProvider = new  CustomSortMethodProvider(customSortMethodMapper);
                var customMethodsContext = new CustomMethodsContext(options, customSortMethodProvider);
                var newContext = new StrainerContext(
                    options,
                    context.Filter,
                    context.Sorting,
                    context.Mapper,
                    context.MetadataProvider,
                    customMethodsContext);

                return new ApplicationStrainerProcessor(newContext);
            });

            // Act
            var result = processor.Apply(model, _posts);
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
            var processor = Factory.CreateProcessor((context) =>
            {
                context.Options.ThrowExceptions = true;

                return new ApplicationStrainerProcessor(context);
            });

            // Assert
            Assert.Throws<StrainerMethodNotFoundException>(() => processor.Apply(model, _posts));
        }

        [Fact]
        public void OrNameFilteringWorks()
        {
            // Arrange
            var model = new StrainerModel()
            {
                Filters = "(Title|LikeCount)==3",
            };
            var processor = Factory.CreateProcessor((context) => new ApplicationStrainerProcessor(context));

            // Act
            var result = processor.Apply(model, _posts);

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
            var processor = Factory.CreateProcessor((context) => new ApplicationStrainerProcessor(context));

            // Act
            var result = processor.Apply(model, _posts);

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
            var processor = Factory.CreateProcessor((context) => new ApplicationStrainerProcessor(context));

            // Act
            var result = processor.Apply(model, _comments);

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
            var processor = Factory.CreateProcessor((context) => new ApplicationStrainerProcessor(context));

            // Act
            var result = processor.Apply(model, _posts);

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
            var processor = Factory.CreateProcessor((context) => new ApplicationStrainerProcessor(context));

            // Act
            var result = processor.Apply(model, _posts);

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
            var processor = Factory.CreateProcessor((context) => new ApplicationStrainerProcessor(context));

            // Act
            var result = processor.Apply(model, _posts);

            // Assert
            result.Should().OnlyContain(p =>
                p.TopComment.Text.Contains("B", StringComparison.OrdinalIgnoreCase)
                || p.FeaturedComment.Text.Contains("B", StringComparison.OrdinalIgnoreCase)
            );
        }
    }
}
