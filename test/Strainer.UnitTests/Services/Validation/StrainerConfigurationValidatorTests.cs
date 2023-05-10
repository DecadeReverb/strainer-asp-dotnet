using Fluorite.Strainer.Models.Configuration;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Validation;
using Moq;

namespace Fluorite.Strainer.UnitTests.Services.Validation
{
    public class StrainerConfigurationValidatorTests
    {
        private readonly Mock<ISortExpressionValidator> _sortExpressionValidatorMock = new();
        private readonly Mock<IFilterOperatorValidator> _filterOperatorValidatorMock = new();

        private readonly StrainerConfigurationValidator _validator;

        public StrainerConfigurationValidatorTests()
        {
            _validator = new StrainerConfigurationValidator(
                _filterOperatorValidatorMock.Object,
                _sortExpressionValidatorMock.Object);
        }

        [Fact]
        public void Should_Throw_ForNullConfiguration()
        {
            // Act
            Action act = () => _validator.Validate(strainerConfiguration: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Should_Throw_WhenOneOfValidatorsThrows()
        {
            // Arrange
            var filterOperators = new Dictionary<string, IFilterOperator>();
            var propertyMetadata = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>();

            var strainerConfigurationMock = new Mock<IStrainerConfiguration>();
            strainerConfigurationMock
                .SetupGet(x => x.FilterOperators)
                .Returns(filterOperators);
            strainerConfigurationMock
                .SetupGet(x => x.PropertyMetadata)
                .Returns(propertyMetadata);
            _sortExpressionValidatorMock
                .Setup(x => x.Validate(propertyMetadata))
                .Throws<Exception>();

            // Act
            Action act = () => _validator.Validate(strainerConfigurationMock.Object);

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage("Invalid Strainer configuration. See inner exception for details.")
                .WithInnerExceptionExactly<Exception>();
        }

        [Fact]
        public void Should_NotThrow_WhenNoValidatorThrows()
        {
            // Arrange
            var filterOperators = new Dictionary<string, IFilterOperator>();
            var propertyMetadata = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>();

            var strainerConfigurationMock = new Mock<IStrainerConfiguration>();
            strainerConfigurationMock
                .SetupGet(x => x.FilterOperators)
                .Returns(filterOperators);
            strainerConfigurationMock
                .SetupGet(x => x.PropertyMetadata)
                .Returns(propertyMetadata);

            // Act
            Action act = () => _validator.Validate(strainerConfigurationMock.Object);

            // Assert
            act.Should().NotThrow<InvalidOperationException>();
        }
    }
}
