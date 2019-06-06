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
            IFilterOperatorProvider provider = new FilterOperatorProvider();
            IFilterOperatorParser parser = new FilterOperatorParser(provider);

            // Act
            var defaultFilterOperator = provider.GetDefaultOperator();
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
            IFilterOperatorProvider provider = new FilterOperatorProvider();
            IFilterOperatorParser parser = new FilterOperatorParser(provider);

            // Act
            var defaultFilterOperator = provider.GetDefaultOperator();
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
            IFilterOperatorProvider provider = new FilterOperatorProvider();
            IFilterOperatorParser parser = new FilterOperatorParser(provider);

            // Act
            var defaultFilterOperator = provider.GetDefaultOperator();
            var filterOperator = parser.GetParsedOperator(input);

            // Assert
            filterOperator
                .Should()
                .Be(defaultFilterOperator);
        }
    }
}
