using FluentAssertions;
using Fluorite.Strainer.Services.Filter;
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
            IFilterOperatorValidator validator = new FilterOperatorValidator();
            IFilterOperatorMapper mapper = new FilterOperatorMapper(validator);
            IFilterOperatorParser parser = new FilterOperatorParser(mapper);

            // Act
            var defaultFilterOperator = mapper.GetDefault();
            var filterOperator = parser.GetParsedOperator(input);

            // Assert
            filterOperator
                .Should()
                .Be(defaultFilterOperator);
        }

        [Fact]
        public void Parser_ReturnsDefaultFilterOperator_WhenInputIsEmpty()
        {
            // Arrange
            var input = string.Empty;
            IFilterOperatorValidator validator = new FilterOperatorValidator();
            IFilterOperatorMapper mapper = new FilterOperatorMapper(validator);
            IFilterOperatorParser parser = new FilterOperatorParser(mapper);

            // Act
            var defaultFilterOperator = mapper.GetDefault();
            var filterOperator = parser.GetParsedOperator(input);

            // Assert
            filterOperator
                .Should()
                .Be(defaultFilterOperator);
        }

        [Fact]
        public void Parser_ReturnsDefaultFilterOperator_WhenInputIsWhitespace()
        {
            // Arrange
            var input = " ";
            IFilterOperatorValidator validator = new FilterOperatorValidator();
            IFilterOperatorMapper mapper = new FilterOperatorMapper(validator);
            IFilterOperatorParser parser = new FilterOperatorParser(mapper);

            // Act
            var defaultFilterOperator = mapper.GetDefault();
            var filterOperator = parser.GetParsedOperator(input);

            // Assert
            filterOperator
                .Should()
                .Be(defaultFilterOperator);
        }
    }
}
