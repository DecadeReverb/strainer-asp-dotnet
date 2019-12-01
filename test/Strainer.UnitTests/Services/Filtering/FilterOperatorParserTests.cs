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
            var validator = new FilterOperatorValidator();
            var mapper = new FilterOperatorMapper(validator);
            var parser = new FilterOperatorParser(mapper);

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
            var validator = new FilterOperatorValidator();
            var mapper = new FilterOperatorMapper(validator);
            var parser = new FilterOperatorParser(mapper);

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
            var validator = new FilterOperatorValidator();
            var mapper = new FilterOperatorMapper(validator);
            var parser = new FilterOperatorParser(mapper);

            // Act
            var filterOperator = parser.GetParsedOperator(symbol);

            // Assert
            filterOperator.Should().BeNull();
        }

        [Fact]
        public void Parser_ReturnsOperators_For_AllFilterOperator_InMapper()
        {
            // Arrange
            var validator = new FilterOperatorValidator();
            var mapper = new FilterOperatorMapper(validator);
            var parser = new FilterOperatorParser(mapper);

            // Act
            var filterOperators = mapper.Symbols.Select(symbol => parser.GetParsedOperator(symbol));

            // Assert
            filterOperators.Should().BeEquivalentTo(mapper.Operators);
        }
    }
}
