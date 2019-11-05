using FluentAssertions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Sorting.Terms;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Sorting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Sorting
{
    public class SortExpressionProviderTests
    {
        [Fact]
        public void Provider_Returns_ListOfSortExpressions()
        {
            // Arrange
            var sortTerm = new SortTerm
            {
                Input = "Text",
                IsDescending = false,
                Name = "Text"
            };
            var propertyInfo = typeof(Comment).GetProperty(nameof(Comment.Text));
            var sortTerms = new Dictionary<PropertyInfo, ISortTerm>
            {
                { propertyInfo, sortTerm },
            };
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new PropertyMapper(optionsProvider);
            mapper.Property<Comment>(c => c.Text).IsSortable();
            var metadataProvider = new AttributePropertyMetadataProvider(mapper, optionsProvider);
            var expressionProvider = new SortExpressionProvider(mapper, metadataProvider);

            // Act
            var sortExpressions = expressionProvider.GetExpressions<Comment>(sortTerms);
            var firstExpression = sortExpressions.FirstOrDefault();

            // Assert
            sortExpressions.Should().NotBeEmpty();
            firstExpression.IsDescending.Should().BeFalse();
            firstExpression.IsSubsequent.Should().BeFalse();
        }

        [Fact]
        public void Provider_Returns_EmptyListOfSortExpressions_When_NoMatchingPropertyIsFound()
        {
            // Arrange
            var sortTerm = new SortTerm
            {
                Input = "Text",
                IsDescending = false,
                Name = "Text"
            };
            var propertyInfo = typeof(Comment).GetProperty(nameof(Comment.Text));
            var sortTerms = new Dictionary<PropertyInfo, ISortTerm>
            {
                { propertyInfo, sortTerm },
            };
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new PropertyMapper(optionsProvider);
            var metadataProvider = new AttributePropertyMetadataProvider(mapper, optionsProvider);
            var expressionProvider = new SortExpressionProvider(mapper, metadataProvider);

            // Act
            var sortExpressions = expressionProvider.GetExpressions<Comment>(sortTerms);

            // Assert
            sortExpressions.Should().BeEmpty();
        }

        [Fact]
        public void Provider_Returns_ListOfSortExpressions_For_NestedProperty()
        {
            // Arrange
            var sortTerm = new SortTerm
            {
                Input = "-TopComment.Text.Length",
                IsDescending = true,
                Name = "TopComment.Text.Length"
            };
            var propertyInfo = typeof(string).GetProperty(nameof(string.Length));
            var sortTerms = new Dictionary<PropertyInfo, ISortTerm>
            {
                { propertyInfo, sortTerm },
            };
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new PropertyMapper(optionsProvider);
            mapper.Property<Post>(c => c.TopComment.Text.Length).IsSortable();
            var metadataProvider = new AttributePropertyMetadataProvider(mapper, optionsProvider);
            var expressionProvider = new SortExpressionProvider(mapper, metadataProvider);

            // Act
            var sortExpressions = expressionProvider.GetExpressions<Post>(sortTerms);
            var firstExpression = sortExpressions.FirstOrDefault();

            // Assert
            sortExpressions.Should().NotBeEmpty();
            firstExpression.IsDescending.Should().BeTrue();
            firstExpression.IsSubsequent.Should().BeFalse();
        }

        [Fact]
        public void Provider_Returns_ListOfSortExpressions_For_SubsequentSorting()
        {
            // Arrange
            var termsList = new List<ISortTerm>
            {
                new SortTerm
                {
                    Input = "-Text,Id",
                    IsDescending = true,
                    Name = "Text"
                },
                new SortTerm
                {
                    Input = "-Text,Id",
                    IsDescending = false,
                    Name = "Id"
                },
            };
            var properties = new PropertyInfo[]
            {
                typeof(Comment).GetProperty(nameof(Comment.Text)),
                typeof(Comment).GetProperty(nameof(Comment.Id)),
            };
            var sortTerms = properties.Zip(termsList, (prop, term) => new { prop, term })
                .ToDictionary(x => x.prop, x => x.term);
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new PropertyMapper(optionsProvider);
            mapper.Property<Comment>(c => c.Text).IsSortable();
            mapper.Property<Comment>(c => c.Id).IsSortable();
            mapper.Property<Comment>(c => c.DateCreated).IsSortable();
            var metadataProvider = new AttributePropertyMetadataProvider(mapper, optionsProvider);
            var expressionProvider = new SortExpressionProvider(mapper, metadataProvider);

            // Act
            var sortExpressions = expressionProvider.GetExpressions<Comment>(sortTerms);
            var firstExpression = sortExpressions.FirstOrDefault();
            var secondExpression = sortExpressions.LastOrDefault();

            // Assert
            sortExpressions.Should().HaveCount(2);
            firstExpression.IsDescending.Should().BeTrue();
            firstExpression.IsSubsequent.Should().BeFalse();
            secondExpression.IsDescending.Should().BeFalse();
            secondExpression.IsSubsequent.Should().BeTrue();
        }

        private class Comment
        {
            [StrainerProperty(IsFilterable = true, IsSortable = true)]
            public DateTimeOffset DateCreated { get; set; }

            public int Id { get; set; }

            [StrainerProperty(IsFilterable = true)]
            public string Text { get; set; }
        }

        private class Post
        {
            public Comment TopComment { get; set; }
        }
    }
}
