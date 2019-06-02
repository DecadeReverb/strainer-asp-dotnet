using FluentAssertions;
using Fluorite.Strainer.Services.Sorting;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Sorting
{
    public class SortTermParserTests
    {
        [Fact]
        public void Parser_ReturnsSortTerm_WhenInputIsNull()
        {
            // Arrange
            string input = null;
            ISortTermParser parser = new SortTermParser();

            // Act
            var sortTermList = parser.GetParsedTerms(input);

            // Assert
            sortTermList
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Parser_ReturnsSortTerm_WhenInputIsEmpty()
        {
            // Arrange
            var input = string.Empty;
            ISortTermParser parser = new SortTermParser();

            // Act
            var sortTermList = parser.GetParsedTerms(input);

            // Assert
            sortTermList
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Parser_ReturnsSortTerm_WhenInputIsOnlyWhitespace()
        {
            // Arrange
            var input = string.Empty;
            ISortTermParser parser = new SortTermParser();

            // Act
            var sortTermList = parser.GetParsedTerms(input);

            // Assert
            sortTermList
                .Should()
                .BeEmpty();
        }
    }
}
