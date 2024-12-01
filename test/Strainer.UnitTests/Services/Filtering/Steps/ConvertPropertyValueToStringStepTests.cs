using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Filtering.Terms;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Filtering.Steps;
using NSubstitute.ReceivedExtensions;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.UnitTests.Services.Filtering.Steps;

public class ConvertPropertyValueToStringStepTests
{
    private readonly ConvertPropertyValueToStringStep _step;

    public ConvertPropertyValueToStringStepTests()
    {
        _step = new ConvertPropertyValueToStringStep();
    }

    [Fact]
    public void Should_Do_Nothing_WhenTermOperator_IsNot_StringBased()
    {
        // Arrange
        var filterOperator = Substitute.For<IFilterOperator>();
        filterOperator
            .IsStringBased
            .Returns(false);
        var term = Substitute.For<IFilterTerm>();
        term
            .Operator
            .Returns(filterOperator);
        var propertyInfo = Substitute.For<PropertyInfo>();
        var propertyMetadata = Substitute.For<IPropertyMetadata>();
        propertyMetadata
            .PropertyInfo
            .Returns(propertyInfo);
        var context = new FilterExpressionWorkflowContext
        {
            PropertyMetadata = propertyMetadata,
            Term = term,
        };

        // Act
        _step.Execute(context);

        // Assert
        context.PropertyValue.Should().BeNull();

        _ = filterOperator
            .Received(1)
            .IsStringBased;
    }

    [Fact]
    public void Should_Do_Nothing_WhenPropertyType_IsAlreadyString()
    {
        // Arrange
        var filterOperator = Substitute.For<IFilterOperator>();
        filterOperator
            .IsStringBased
            .Returns(true);
        var term = Substitute.For<IFilterTerm>();
        term
            .Operator
            .Returns(filterOperator);
        var propertyInfo = typeof(Blog).GetProperty(nameof(Blog.Title));
        var propertyMetadata = Substitute.For<IPropertyMetadata>();
        propertyMetadata
            .PropertyInfo
            .Returns(propertyInfo);
        var context = new FilterExpressionWorkflowContext
        {
            PropertyMetadata = propertyMetadata,
            Term = term,
        };

        // Act
        _step.Execute(context);

        // Assert
        context.PropertyValue.Should().BeNull();

        _ = filterOperator
            .Received(1)
            .IsStringBased;
    }

    [Fact]
    public void Should_Set_PropertyValue_As_ToStringMethodCall()
    {
        // Arrange
        var filterOperator = Substitute.For<IFilterOperator>();
        filterOperator
            .IsStringBased
            .Returns(true);
        var term = Substitute.For<IFilterTerm>();
        term
            .Operator
            .Returns(filterOperator);
        var propertyInfo = typeof(Blog).GetProperty(nameof(Blog.Id));
        var propertyMetadata = Substitute.For<IPropertyMetadata>();
        propertyMetadata
            .PropertyInfo
            .Returns(propertyInfo);
        var context = new FilterExpressionWorkflowContext
        {
            PropertyMetadata = propertyMetadata,
            PropertyValue = Expression.Constant(1),
            Term = term,
        };

        // Act
        _step.Execute(context);

        // Assert
        context.PropertyValue.Should().NotBeNull();
        context.PropertyValue.Should().BeAssignableTo<MethodCallExpression>();
        var methodCallExpression = context.PropertyValue as MethodCallExpression;
        methodCallExpression.Arguments.Should().BeEmpty();
        methodCallExpression.Method.Should().BeSameAs(typeof(object).GetMethod(nameof(ToString)));

        _ = filterOperator
            .Received(1)
            .IsStringBased;
    }

    private class Blog
    {
        public int Id { get; set; }

        public string Title { get; set; }
    }
}
