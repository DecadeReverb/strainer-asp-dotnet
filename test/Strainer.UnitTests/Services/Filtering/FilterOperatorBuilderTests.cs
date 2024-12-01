using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Services.Filtering;
using System.Linq.Expressions;

namespace Fluorite.Strainer.UnitTests.Services.Filtering;

public class FilterOperatorBuilderTests
{
    [Fact]
    public void Builder_Adds_Operator_WithSymbol_And_Expression()
    {
        // Arrange
        var symbol = "===";
        var name = "test";
        var expression = Expression.Empty();

        // Act
        var filterOperator = new FilterOperatorBuilder(symbol)
            .HasName(name)
            .HasExpression(_ => expression)
            .Build();

        // Assert
        filterOperator.Should().NotBeNull();
        filterOperator.Symbol.Should().Be(symbol);
        filterOperator.Name.Should().Be(name);
        filterOperator.IsCaseInsensitive.Should().BeFalse();
        filterOperator.IsStringBased.Should().BeFalse();
        filterOperator.Expression.Should().NotBeNull();
        filterOperator.Expression.Invoke(Substitute.For<IFilterExpressionContext>()).Should().BeSameAs(expression);
    }
}
