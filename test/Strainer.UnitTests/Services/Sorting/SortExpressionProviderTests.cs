using FluentAssertions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Sorting.Terms;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Sorting;
using Fluorite.Strainer.UnitTests.Entities;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Sorting
{
    public class SortExpressionProviderTests
    {
        [Fact]
        public void Provider_ReturnsListOfSortExpressions()
        {
            // Arrange
            var sortTerms = new List<ISortTerm>
            {
                new SortTerm
                {
                    Input = "Text",
                    IsDescending = false,
                    Name = "Text"
                },
            };
            var mapper = new StrainerPropertyMapper();
            mapper.Property<Comment>(c => c.Text).CanSort();
            var options = Options.Create(new StrainerOptions());
            var provider = new SortExpressionProvider(mapper, options);

            // Act
            var sortExpressions = provider.GetExpressions<Comment>(sortTerms);
            var firstExpression = sortExpressions.FirstOrDefault();

            // Assert
            sortExpressions.Should().NotBeEmpty();
            firstExpression.IsDescending.Should().BeFalse();
            firstExpression.IsSubsequent.Should().BeFalse();
        }

        [Fact]
        public void Provider_ReturnsEmptyListOfSortExpressions_WhenNoMatchingPropertyIsFound()
        {
            // Arrange
            var sortTerms = new List<ISortTerm>
            {
                new SortTerm
                {
                    Input = "Text",
                    IsDescending = false,
                    Name = "Text"
                },
            };
            var mapper = new StrainerPropertyMapper();
            var options = Options.Create(new StrainerOptions());
            var provider = new SortExpressionProvider(mapper, options);

            // Act
            var sortExpressions = provider.GetExpressions<Comment>(sortTerms);

            // Assert
            sortExpressions.Should().BeEmpty();
        }

        [Fact]
        public void Provider_ReturnsListOfSortExpressions_ForNestedProperty()
        {
            // Arrange
            var sortTerms = new List<ISortTerm>
            {
                new SortTerm
                {
                    Input = "-TopComment.Text.Length",
                    IsDescending = true,
                    Name = "TopComment.Text.Length"
                },
            };
            var mapper = new StrainerPropertyMapper();
            mapper.Property<Post>(c => c.TopComment.Text.Length).CanSort();
            var options = Options.Create(new StrainerOptions());
            var provider = new SortExpressionProvider(mapper, options);

            // Act
            var sortExpressions = provider.GetExpressions<Post>(sortTerms);
            var firstExpression = sortExpressions.FirstOrDefault();

            // Assert
            sortExpressions.Should().NotBeEmpty();
            firstExpression.IsDescending.Should().BeTrue();
            firstExpression.IsSubsequent.Should().BeFalse();
        }

        [Fact]
        public void Provider_ReturnsListOfSortExpressions_ForSubsequentSorting()
        {
            // Arrange
            var sortTerms = new List<ISortTerm>
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
            var mapper = new StrainerPropertyMapper();
            mapper.Property<Comment>(c => c.Text).CanSort();
            mapper.Property<Comment>(c => c.Id).CanSort();
            mapper.Property<Comment>(c => c.DateCreated).CanSort();
            var options = Options.Create(new StrainerOptions());
            var provider = new SortExpressionProvider(mapper, options);

            // Act
            var sortExpressions = provider.GetExpressions<Comment>(sortTerms);
            var firstExpression = sortExpressions.FirstOrDefault();
            var secondExpression = sortExpressions.LastOrDefault();

            // Assert
            sortExpressions.Count.Should().Be(2);
            firstExpression.IsDescending.Should().BeTrue();
            firstExpression.IsSubsequent.Should().BeFalse();
            secondExpression.IsDescending.Should().BeFalse();
            secondExpression.IsSubsequent.Should().BeTrue();
        }
    }
}
