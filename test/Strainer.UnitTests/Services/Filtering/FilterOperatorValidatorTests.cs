using Fluorite.Strainer.Collections;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Validation;
using System.Linq.Expressions;

namespace Fluorite.Strainer.UnitTests.Services.Filtering;

public class FilterOperatorValidatorTests
{
    [Fact]
    public void Validator_DoesNot_Throw_Exception_For_ValidOperator()
    {
        // Arrange
        var filterOperator = new FilterOperator
        {
            Expression = (context) => Expression.Empty(),
            Symbol = "@",
        };
        var validator = new FilterOperatorValidator();

        // Act & Assert
        Action action = () => validator.Validate(filterOperator);
        action.Should().NotThrow();
    }

    [Fact]
    public void Validator_Throw_Exception_For_Operator_With_NullSymbol()
    {
        // Arrange
        var filterOperator = new FilterOperator
        {
            Expression = (context) => Expression.Empty(),
            Symbol = null,
        };
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
        var filterOperator = new FilterOperator
        {
            Expression = (context) => Expression.Empty(),
            Symbol = string.Empty,
        };
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
        var filterOperator = new FilterOperator
        {
            Expression = (context) => Expression.Empty(),
            Symbol = " ",
        };
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
        var filterOperator = new FilterOperator
        {
            Expression = null,
            Symbol = "@",
        };
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
        var filterOperators = new FilterOperator[]
        {
            new FilterOperator
            {
                Expression = (context) => Expression.Empty(),
                Symbol = "@",
            },
            new FilterOperator
            {
                Expression = (context) => Expression.Empty(),
                Symbol = "@",
            }
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
        var excludedBuiltInFilterOperators = new ReadOnlyHashSet<string>(new[]
        {
            symbol,
        });
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
