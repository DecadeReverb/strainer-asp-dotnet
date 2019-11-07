using FluentAssertions;
using Fluorite.Strainer.Services.Sorting;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Sorting
{
    public class SortTermParserTests
    {
        [Fact]
        public void Parser_ReturnsNoSortTerm_WhenInputIsNull()
        {
            // Arrange
            string input = null;
            ISortingWayFormatter formatter = new DescendingPrefixSortingWayFormatter();
            ISortTermParser parser = new SortTermParser(formatter);

            // Act
            var sortTermList = parser.GetParsedTerms(input);

            // Assert
            sortTermList
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Parser_ReturnsNoSortTerm_WhenInputIsEmpty()
        {
            // Arrange
            var input = string.Empty;
            ISortingWayFormatter formatter = new DescendingPrefixSortingWayFormatter();
            ISortTermParser parser = new SortTermParser(formatter);

            // Act
            var sortTermList = parser.GetParsedTerms(input);

            // Assert
            sortTermList
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Parser_ReturnsNoSortTerm_WhenInputIsOnlyWhitespace()
        {
            // Arrange
            var input = string.Empty;
            ISortingWayFormatter formatter = new DescendingPrefixSortingWayFormatter();
            ISortTermParser parser = new SortTermParser(formatter);

            // Act
            var sortTermList = parser.GetParsedTerms(input);

            // Assert
            sortTermList
                .Should()
                .BeEmpty();
        }
    }
}
