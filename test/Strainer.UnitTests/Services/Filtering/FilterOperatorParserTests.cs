using FluentAssertions;
using Fluorite.Strainer.Services.Filtering;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Filtering
{
    public class FilterOperatorParserTests
    {
        [Fact]
        public void Parser_ReturnsNullFilterOperator_WhenInputIsNull()
        {
            // Arrange
            string symbol = null;
            var filterOperators = FilterOperatorMapper.DefaultOperators
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            var dictionary = new FilterOperatorDictionary(filterOperators);
            var parser = new FilterOperatorParser(dictionary);

            // Act
            var filterOperator = parser.GetParsedOperator(symbol);

            // Assert
            filterOperator.Should().BeNull();
        }

        [Fact]
        public void Parser_ReturnsDefaultFilterOperator_WhenInputIsEmpty()
        {
            // Arrange
            var symbol = string.Empty;
            var filterOperators = FilterOperatorMapper.DefaultOperators
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            var dictionary = new FilterOperatorDictionary(filterOperators);
            var parser = new FilterOperatorParser(dictionary);

            // Act
            var filterOperator = parser.GetParsedOperator(symbol);

            // Assert
            filterOperator.Should().BeNull();
        }

        [Fact]
        public void Parser_ReturnsDefaultFilterOperator_WhenInputIsWhitespace()
        {
            // Arrange
            var symbol = " ";
            var filterOperators = FilterOperatorMapper.DefaultOperators
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            var dictionary = new FilterOperatorDictionary(filterOperators);
            var parser = new FilterOperatorParser(dictionary);

            // Act
            var filterOperator = parser.GetParsedOperator(symbol);

            // Assert
            filterOperator.Should().BeNull();
        }

        [Fact]
        public void Parser_ReturnsOperators_For_AllFilterOperator_InMapper()
        {
            // Arrange
            var filterOperators = FilterOperatorMapper.DefaultOperators
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            var dictionary = new FilterOperatorDictionary(filterOperators);
            var parser = new FilterOperatorParser(dictionary);

            // Act
            var result = filterOperators.Keys.Select(symbol => parser.GetParsedOperator(symbol));

            // Assert
            result.Should().BeEquivalentTo(filterOperators.Values);
        }
    }
}
