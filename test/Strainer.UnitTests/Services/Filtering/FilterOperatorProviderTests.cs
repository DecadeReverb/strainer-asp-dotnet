using System.Linq;
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
        public void Provider_ReturnsNull_WhenNoMatchingOperatorIsFound()
        {
            // Arrange
            var @operator = string.Empty;
            IFilterOperatorProvider provider = new FilterOperatorProvider();

            // Act
            var filterOperator = provider.FirstOrDefault(f => f.Operator == @operator);

            // Assert
            filterOperator
                .Should()
                .BeNull();
        }

        [Fact]
        public void Provider_IsNotEmpty()
        {
            // Arrange
            IFilterOperatorProvider provider = new FilterOperatorProvider();

            // Act
            var filterOperatorsAmount = provider.Count();

            // Assert
            filterOperatorsAmount
                .Should()
                .BeGreaterThan(0);
        }
    }
}
