using FluentAssertions;
using Fluorite.Strainer.Services.Filtering;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Filtering
{
    public class FilterOperatorMapperTests
    {
        [Fact]
        public void Mapper_ReturnsDefaultFilterOperator()
        {
            // Arrange
            IFilterOperatorValidator validator = new FilterOperatorValidator();
            IFilterOperatorMapper mapper = new FilterOperatorMapper(validator);

            // Act
            var defaultFilterOperator = mapper.GetDefault();

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
        public void Mapper_ReturnsNull_WhenNoMatchingOperatorIsFound()
        {
            // Arrange
            var symbol = string.Empty;
            IFilterOperatorValidator validator = new FilterOperatorValidator();
            IFilterOperatorMapper mapper = new FilterOperatorMapper(validator);

            // Act
            var filterOperator = mapper.Find(symbol);

            // Assert
            filterOperator
                .Should()
                .BeNull();
        }

        [Fact]
        public void Mapper_IsNotEmpty()
        {
            // Arrange
            IFilterOperatorValidator validator = new FilterOperatorValidator();
            IFilterOperatorMapper mapper = new FilterOperatorMapper(validator);

            // Act
            var filterOperatorsAmount = mapper.Operators.Count;

            // Assert
            filterOperatorsAmount
                .Should()
                .BeGreaterThan(0);
        }
    }
}
