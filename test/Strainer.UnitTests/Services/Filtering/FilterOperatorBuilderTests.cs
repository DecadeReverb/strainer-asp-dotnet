using FluentAssertions;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Services.Filtering;
using System.Linq.Expressions;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Filtering
{
    public class FilterOperatorBuilderTests
    {
        [Fact]
        public void Builder_Adds_Operator_WithSymbol()
        {
            // Arrange
            var validator = new FilterOperatorValidator();
            var mapper = new FilterOperatorMapper(validator);

            // Act
            new FilterOperatorBuilder(mapper, symbol: "===");

            // Assert
            mapper.Symbols.Should().Contain("===");
        }

        [Fact]
        public void Builder_Adds_Operator_WithSymbol_And_Expression()
        {
            // Arrange
            var validator = new FilterOperatorValidator();
            var mapper = new FilterOperatorMapper(validator);

            // Act
            var filterOperator = new FilterOperatorBuilder(mapper, symbol: "===")
                .HasExpression(context => Expression.Empty())
                .Build();

            // Assert
            mapper.Symbols.Should().Contain("===");
            mapper.Operators.Should().Contain(f => f.Expression == filterOperator.Expression);
        }
    }
}
