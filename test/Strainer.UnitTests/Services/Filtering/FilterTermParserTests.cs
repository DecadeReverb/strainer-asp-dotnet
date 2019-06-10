using FluentAssertions;
using Fluorite.Strainer.Services.Filtering;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Filtering
{
    public class FilterTermParserTests
    {
        [Fact]
        public void Parser_ReturnsNoFilterTerms_WhenInputIsNull()
        {
            // Arrange
            string input = null;
            IFilterOperatorProvider operatorProvider = new FilterOperatorProvider();
            IFilterOperatorParser operatorParser = new FilterOperatorParser(operatorProvider);
            IFilterTermParser parser = new FilterTermParser(operatorParser, operatorProvider);

            // Act
            var FilterTermList = parser.GetParsedTerms(input);

            // Assert
            FilterTermList
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Parser_ReturnsNoFilterTerms_WhenInputIsEmpty()
        {
            // Arrange
            var input = string.Empty;
            IFilterOperatorProvider operatorProvider = new FilterOperatorProvider();
            IFilterOperatorParser operatorParser = new FilterOperatorParser(operatorProvider);
            IFilterTermParser parser = new FilterTermParser(operatorParser, operatorProvider);

            // Act
            var FilterTermList = parser.GetParsedTerms(input);

            // Assert
            FilterTermList
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Parser_ReturnsNoFilterTerms_WhenInputIsOnlyWhitespace()
        {
            // Arrange
            var input = string.Empty;
            IFilterOperatorProvider operatorProvider = new FilterOperatorProvider();
            IFilterOperatorParser operatorParser = new FilterOperatorParser(operatorProvider);
            IFilterTermParser parser = new FilterTermParser(operatorParser, operatorProvider);

            // Act
            var FilterTermList = parser.GetParsedTerms(input);

            // Assert
            FilterTermList
                .Should()
                .BeEmpty();
        }
    }
}
