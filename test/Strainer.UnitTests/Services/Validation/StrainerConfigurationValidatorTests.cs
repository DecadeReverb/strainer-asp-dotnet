using Fluorite.Strainer.Models.Configuration;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Validation;

namespace Fluorite.Strainer.UnitTests.Services.Validation;

public class StrainerConfigurationValidatorTests
{
    private readonly ISortExpressionValidator _sortExpressionValidatorMock = Substitute.For<ISortExpressionValidator>();
    private readonly IFilterOperatorValidator _filterOperatorValidatorMock = Substitute.For<IFilterOperatorValidator>();

    private readonly StrainerConfigurationValidator _validator;

    public StrainerConfigurationValidatorTests()
    {
        _validator = new StrainerConfigurationValidator(
            _filterOperatorValidatorMock,
            _sortExpressionValidatorMock);
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

        var strainerConfigurationMock = Substitute.For<IStrainerConfiguration>();
        strainerConfigurationMock
            .FilterOperators
            .Returns(filterOperators);
        strainerConfigurationMock
            .PropertyMetadata
            .Returns(propertyMetadata);
        _sortExpressionValidatorMock
            .When(x => x.Validate(propertyMetadata))
            .Throw<Exception>();

        // Act
        Action act = () => _validator.Validate(strainerConfigurationMock);

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

        var strainerConfigurationMock = Substitute.For<IStrainerConfiguration>();
        strainerConfigurationMock
            .FilterOperators
            .Returns(filterOperators);
        strainerConfigurationMock
            .PropertyMetadata
            .Returns(propertyMetadata);

        // Act
        Action act = () => _validator.Validate(strainerConfigurationMock);

        // Assert
        act.Should().NotThrow<InvalidOperationException>();
    }
}
