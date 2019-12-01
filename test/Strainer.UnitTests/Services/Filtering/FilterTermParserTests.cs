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
            var validator = new FilterOperatorValidator();
            var mapper = new FilterOperatorMapper(validator);
            var operatorParser = new FilterOperatorParser(mapper);
            var parser = new FilterTermParser(operatorParser, mapper);

            // Act
            var filterTermList = parser.GetParsedTerms(input);

            // Assert
            filterTermList.Should().BeEmpty();
        }

        [Fact]
        public void Parser_ReturnsNoFilterTerms_WhenInputIsEmpty()
        {
            // Arrange
            var input = string.Empty;
            var validator = new FilterOperatorValidator();
            var mapper = new FilterOperatorMapper(validator);
            var operatorParser = new FilterOperatorParser(mapper);
            var parser = new FilterTermParser(operatorParser, mapper);

            // Act
            var filterTermList = parser.GetParsedTerms(input);

            // Assert
            filterTermList.Should().BeEmpty();
        }

        [Fact]
        public void Parser_ReturnsNoFilterTerms_WhenInputIsOnlyWhitespace()
        {
            // Arrange
            var input = string.Empty;
            var validator = new FilterOperatorValidator();
            var mapper = new FilterOperatorMapper(validator);
            var operatorParser = new FilterOperatorParser(mapper);
            var parser = new FilterTermParser(operatorParser, mapper);

            // Act
            var filterTermList = parser.GetParsedTerms(input);

            // Assert
            filterTermList.Should().BeEmpty();
        }
    }
}
