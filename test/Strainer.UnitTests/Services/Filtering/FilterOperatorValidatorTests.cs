using Fluorite.Strainer.Collections;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Validation;
using NSubstitute.ReturnsExtensions;
using System.Linq.Expressions;

namespace Fluorite.Strainer.UnitTests.Services.Filtering;

public class FilterOperatorValidatorTests
{
    [Fact]
    public void Validator_DoesNot_Throw_Exception_For_ValidOperator()
    {
        // Arrange
        var filterOperator = new FilterOperator("test", "@", _ => Expression.Empty());
        var validator = new FilterOperatorValidator();

        // Act & Assert
        Action action = () => validator.Validate(filterOperator);
        action.Should().NotThrow();
    }

    [Fact]
    public void Validator_Throw_Exception_For_Operator_With_NullSymbol()
    {
        // Arrange
        var filterOperator = Substitute.For<IFilterOperator>();
        filterOperator.Symbol.ReturnsNull();
        var validator = new FilterOperatorValidator();

        // Act & Assert
        Action action = () => validator.Validate(filterOperator);
        action
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage("*symbol*");
    }

    [Fact]
    public void Validator_Throw_Exception_For_Operator_With_EmptySymbol()
    {
        // Arrange
        var filterOperator = Substitute.For<IFilterOperator>();
        filterOperator.Symbol.Returns(string.Empty);
        var validator = new FilterOperatorValidator();

        // Act & Assert
        Action action = () => validator.Validate(filterOperator);
        action
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage("*symbol*");
    }

    [Fact]
    public void Validator_Throw_Exception_For_Operator_With_WhitespaceSymbol()
    {
        // Arrange
        var filterOperator = Substitute.For<IFilterOperator>();
        filterOperator.Symbol.Returns(" ");
        var validator = new FilterOperatorValidator();

        // Act & Assert
        Action action = () => validator.Validate(filterOperator);
        action
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage("*symbol*");
    }

    [Fact]
    public void Validator_Throw_Exception_For_Operator_With_NullExpression()
    {
        // Arrange
        var filterOperator = Substitute.For<IFilterOperator>();
        filterOperator.Symbol.Returns("@");
        filterOperator.Expression.ReturnsNull();
        var validator = new FilterOperatorValidator();

        // Act & Assert
        Action action = () => validator.Validate(filterOperator);
        action
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage("*expression*");
    }

    [Fact]
    public void Validator_Throw_Exception_For_Operators_With_TheSameSymbol()
    {
        // Arrange
        var filterOperator = Substitute.For<IFilterOperator>();
        filterOperator.Symbol.Returns("@");
        filterOperator.Expression.Returns(_ => Expression.Empty());
        var filterOperators = new IFilterOperator[]
        {
            filterOperator,
            filterOperator,
        };
        var excludedBuiltInFilterOperators = new ReadOnlyHashSet<string>(new HashSet<string>());
        var validator = new FilterOperatorValidator();

        // Act & Assert
        Action action = () => validator.Validate(filterOperators, excludedBuiltInFilterOperators);
        action
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage("*symbol*");
    }

    [Fact]
    public void Validator_Throw_Exception_For_Operators_With_TheSameSymbolAsBuiltIn()
    {
        // Arrange
        var symbol = FilterOperatorSymbols.EqualsSymbol;
        var filterOperators = new IFilterOperator[]
        {
            FilterOperatorMapper.DefaultOperators[symbol],
        };
        var excludedBuiltInFilterOperators = new ReadOnlyHashSet<string>(
        [
            symbol,
        ]);
        var validator = new FilterOperatorValidator();

        // Act & Assert
        Action action = () => validator.Validate(filterOperators, excludedBuiltInFilterOperators);
        action
            .Should()
            .ThrowExactly<InvalidOperationException>()
            .WithMessage(
                $"Excluded built-in filter operator symbol {symbol} detected in configuration values. " +
                $"Please ensure that no such filter operators are supplied when marking them as excluded.");
    }
}
