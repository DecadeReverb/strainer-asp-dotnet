using FluentAssertions;
using Fluorite.Strainer.Services.Filtering;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Filtering
{
    public class FilterOperatorParserTests
    {
        [Fact]
        public void Parser_ReturnsDefaultFilterOperator_WhenInputIsNull()
        {
            // Arrange
            string input = null;
            var validator = new FilterOperatorValidator();
            var mapper = new FilterOperatorMapper(validator);
            var parser = new FilterOperatorParser(mapper);

            // Act
            var defaultFilterOperator = mapper.GetDefault();
            var filterOperator = parser.GetParsedOperator(input);

            // Assert
            filterOperator.Should().Be(defaultFilterOperator);
        }

        [Fact]
        public void Parser_ReturnsDefaultFilterOperator_WhenInputIsEmpty()
        {
            // Arrange
            var input = string.Empty;
            var validator = new FilterOperatorValidator();
            var mapper = new FilterOperatorMapper(validator);
            var parser = new FilterOperatorParser(mapper);

            // Act
            var defaultFilterOperator = mapper.GetDefault();
            var filterOperator = parser.GetParsedOperator(input);

            // Assert
            filterOperator.Should().Be(defaultFilterOperator);
        }

        [Fact]
        public void Parser_ReturnsDefaultFilterOperator_WhenInputIsWhitespace()
        {
            // Arrange
            var input = " ";
            var validator = new FilterOperatorValidator();
            var mapper = new FilterOperatorMapper(validator);
            var parser = new FilterOperatorParser(mapper);

            // Act
            var defaultFilterOperator = mapper.GetDefault();
            var filterOperator = parser.GetParsedOperator(input);

            // Assert
            filterOperator.Should().Be(defaultFilterOperator);
        }
    }
}
