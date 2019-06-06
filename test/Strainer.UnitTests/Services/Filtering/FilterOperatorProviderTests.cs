using FluentAssertions;
using Fluorite.Strainer.Services.Filtering;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Filtering
{
    public class FilterOperatorProviderTests
    {
        [Fact]
        public void Provider_ReturnsDefaultFilterOperator()
        {
            // Arrange
            IFilterOperatorProvider provider = new FilterOperatorProvider();

            // Act
            var defaultFilterOperator = provider.GetDefaultOperator();

            // Assert
            defaultFilterOperator
                .Should()
                .NotBeNull();
            defaultFilterOperator
                .IsDefault
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Provider_ReturnsDefaultFilterOperator_WhenNoMatchingOperatorIsFound()
        {
            // Arrange
            var filterOperator = string.Empty;
            IFilterOperatorProvider provider = new FilterOperatorProvider();

            // Act
            var defaultFilterOperator = provider.GetFirstOrDefault(filterOperator);

            // Assert
            defaultFilterOperator
                .Should()
                .NotBeNull();
            defaultFilterOperator
                .IsDefault
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Provider_ReturnsFilterOperatorList()
        {
            // Arrange
            IFilterOperatorProvider provider = new FilterOperatorProvider();

            // Act
            var filterOperators = provider.Operators;

            // Assert
            filterOperators
                .Should()
                .NotBeEmpty();
        }
    }
}
