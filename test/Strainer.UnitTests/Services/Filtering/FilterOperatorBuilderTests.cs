using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Services.Filtering;
using System.Linq.Expressions;

namespace Fluorite.Strainer.UnitTests.Services.Filtering
{
    public class FilterOperatorBuilderTests
    {
        [Fact]
        public void Builder_Adds_Operator_WithSymbol()
        {
            // Arrange
            var filterOperators = new Dictionary<string, IFilterOperator>();

            // Act
            new FilterOperatorBuilder(filterOperators, symbol: "===");

            // Assert
            filterOperators.Keys.Should().Contain("===");
        }

        [Fact]
        public void Builder_Adds_Operator_WithSymbol_And_Expression()
        {
            // Arrange
            var filterOperators = new Dictionary<string, IFilterOperator>();

            // Act
            var filterOperator = new FilterOperatorBuilder(filterOperators, symbol: "===")
                .HasExpression(context => Expression.Empty())
                .Build();

            // Assert
            filterOperators.Keys.Should().Contain("===");
            filterOperators.Values.Should().Contain(f => f.Expression == filterOperator.Expression);
        }
    }
}
