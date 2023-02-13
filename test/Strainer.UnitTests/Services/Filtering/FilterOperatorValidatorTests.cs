using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Services.Filtering;
using System.Linq.Expressions;

namespace Fluorite.Strainer.UnitTests.Services.Filtering
{
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
                .Throw<InvalidOperationException>()
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
                .Throw<InvalidOperationException>()
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
                .Throw<InvalidOperationException>()
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
                .Throw<InvalidOperationException>()
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
            var validator = new FilterOperatorValidator();

            // Act & Assert
            Action action = () => validator.Validate(filterOperators);
            action
                .Should()
                .Throw<InvalidOperationException>()
                .WithMessage("*symbol*");
        }
    }
}
