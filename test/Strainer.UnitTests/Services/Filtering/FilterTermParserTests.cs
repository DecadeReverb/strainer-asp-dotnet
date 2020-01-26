using FluentAssertions;
using Fluorite.Strainer.Services.Filtering;
using System.Linq;
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
            var filterOperators = FilterOperatorMapper.DefaultOperators
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            var operatorParser = new FilterOperatorParser(filterOperators);
            var parser = new FilterTermParser(operatorParser, filterOperators);

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
            var filterOperators = FilterOperatorMapper.DefaultOperators
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            var operatorParser = new FilterOperatorParser(filterOperators);
            var parser = new FilterTermParser(operatorParser, filterOperators);

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
            var filterOperators = FilterOperatorMapper.DefaultOperators
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            var operatorParser = new FilterOperatorParser(filterOperators);
            var parser = new FilterTermParser(operatorParser, filterOperators);

            // Act
            var filterTermList = parser.GetParsedTerms(input);

            // Assert
            filterTermList.Should().BeEmpty();
        }
    }
}
