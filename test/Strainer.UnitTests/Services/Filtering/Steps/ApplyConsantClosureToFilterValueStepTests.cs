using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Filtering.Terms;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Filtering.Steps;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.UnitTests.Services.Filtering.Steps;

public class ApplyConsantClosureToFilterValueStepTests
{
    private readonly ApplyConsantClosureToFilterValueStep _step;

    public ApplyConsantClosureToFilterValueStepTests()
    {
        _step = new ();
    }

    [Fact]
    public void Should_Assing_StringConstant_WhenFilterOperatorIsStringBased()
    {
        // Arrange
        object value = "foo";
        var operatorMock = Substitute.For<IFilterOperator>();
        operatorMock.IsStringBased.Returns(true);
        var filterTermMock = Substitute.For<IFilterTerm>();
        filterTermMock.Operator.Returns(operatorMock);
        var context = new FilterExpressionWorkflowContext
        {
            FilterTermConstant = value,
            Term = filterTermMock,
        };

        // Act
        _step.Execute(context);

        // Assert
        context.FinalExpression.Should().NotBeNull();
        context.FinalExpression.Should().BeAssignableTo<ConstantExpression>()
            .Subject.Value.Should().Be(value);
    }

    [Fact]
    public void Should_Assing_OtherTypeConstant_WhenFilterOperatorIsNotStringBased()
    {
        // Arrange
        object value = 123;
        var operatorMock = Substitute.For<IFilterOperator>();
        operatorMock.IsStringBased.Returns(false);
        var filterTermMock = Substitute.For<IFilterTerm>();
        filterTermMock.Operator.Returns(operatorMock);
        var propertyInfoMock = Substitute.For<PropertyInfo>();
        propertyInfoMock.PropertyType.Returns(value.GetType());
        var metadataMock = Substitute.For<IPropertyMetadata>();
        metadataMock.PropertyInfo.Returns(propertyInfoMock);
        var context = new FilterExpressionWorkflowContext
        {
            FilterTermConstant = value,
            Term = filterTermMock,
            PropertyMetadata = metadataMock,
        };

        // Act
        _step.Execute(context);

        // Assert
        context.FinalExpression.Should().NotBeNull();
        context.FinalExpression.Should().BeAssignableTo<ConstantExpression>()
            .Subject.Value.Should().Be(value);
    }

    [Fact]
    public void Should_Throw_WhenFilterTermConstantIsNotString_WhenFilterOperatorIsStringBased()
    {
        // Arrange
        object value = 123;
        var operatorMock = Substitute.For<IFilterOperator>();
        operatorMock.IsStringBased.Returns(true);
        var filterTermMock = Substitute.For<IFilterTerm>();
        filterTermMock.Operator.Returns(operatorMock);
        var context = new FilterExpressionWorkflowContext
        {
            FilterTermConstant = value,
            Term = filterTermMock,
        };

        // Act
        Action act = () => _step.Execute(context);

        // Assert
        act.Should().ThrowExactly<InvalidOperationException>()
            .WithMessage("Cannot get a closure over constant using wrong type.*");
    }

    [Fact]
    public void Should_Throw_WhenFilterTermConstantIsNotOfMatchingType_WhenFilterOperatorIsNotStringBased()
    {
        // Arrange
        object value = true;
        var operatorMock = Substitute.For<IFilterOperator>();
        operatorMock.IsStringBased.Returns(false);
        var filterTermMock = Substitute.For<IFilterTerm>();
        filterTermMock.Operator.Returns(operatorMock);
        var propertyInfoMock = Substitute.For<PropertyInfo>();
        propertyInfoMock.PropertyType.Returns(typeof(int));
        var metadataMock = Substitute.For<IPropertyMetadata>();
        metadataMock.PropertyInfo.Returns(propertyInfoMock);
        var context = new FilterExpressionWorkflowContext
        {
            FilterTermConstant = value,
            Term = filterTermMock,
            PropertyMetadata = metadataMock,
        };

        // Act
        Action act = () => _step.Execute(context);

        // Assert
        act.Should().ThrowExactly<InvalidOperationException>()
            .WithMessage("Cannot get a closure over constant using wrong type.*");
    }
}
