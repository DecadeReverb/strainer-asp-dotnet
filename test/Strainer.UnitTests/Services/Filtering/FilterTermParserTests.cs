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
            IFilterOperatorValidator validator = new FilterOperatorValidator();
            IFilterOperatorMapper mapper = new FilterOperatorMapper(validator);
            IFilterOperatorParser operatorParser = new FilterOperatorParser(mapper);
            IFilterTermParser parser = new FilterTermParser(operatorParser, mapper);

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
            IFilterOperatorValidator validator = new FilterOperatorValidator();
            IFilterOperatorMapper mapper = new FilterOperatorMapper(validator);
            IFilterOperatorParser operatorParser = new FilterOperatorParser(mapper);
            IFilterTermParser parser = new FilterTermParser(operatorParser, mapper);

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
            IFilterOperatorValidator validator = new FilterOperatorValidator();
            IFilterOperatorMapper mapper = new FilterOperatorMapper(validator);
            IFilterOperatorParser operatorParser = new FilterOperatorParser(mapper);
            IFilterTermParser parser = new FilterTermParser(operatorParser, mapper);

            // Act
            var FilterTermList = parser.GetParsedTerms(input);

            // Assert
            FilterTermList
                .Should()
                .BeEmpty();
        }
    }
}
