using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Filtering.Terms;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Conversion;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Filtering.Steps;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.UnitTests.Services.Filtering.Steps;

public class ChangeTypeOfFilterValueStepTests
{
    private readonly ITypeChanger _typeChangeMock = Substitute.For<ITypeChanger>();

    private readonly ChangeTypeOfFilterValueStep _step;

    public ChangeTypeOfFilterValueStepTests()
    {
        _step = new ChangeTypeOfFilterValueStep(_typeChangeMock);
    }

    [Fact]
    public void Should_DoNothing_When_FilterTermOperator_IsStringBased()
    {
        // Arrange
        var filterOperator = Substitute.For<IFilterOperator>();
        filterOperator.IsStringBased.Returns(true);
        var term = Substitute.For<IFilterTerm>();
        term.Operator.Returns(filterOperator);
        var context = new FilterExpressionWorkflowContext
        {
            Term = term,
        };

        // Act
        _step.Execute(context);

        // Assert
        _typeChangeMock
            .DidNotReceive()
            .ChangeType(Arg.Any<string>(), Arg.Any<Type>());
    }

    [Fact]
    public void Should_Convert_When_PropertyType_IsString()
    {
        // Arrange
        var filterValue = "foo";
        var filterOperator = Substitute.For<IFilterOperator>();
        filterOperator.IsStringBased.Returns(false);
        var term = Substitute.For<IFilterTerm>();
        term.Operator.Returns(filterOperator);
        var propertyType = typeof(string);
        var propertyInfo = Substitute.For<PropertyInfo>();
        propertyInfo.PropertyType.Returns(propertyType);
        var propertyMetadata = Substitute.For<IPropertyMetadata>();
        propertyMetadata.PropertyInfo.Returns(propertyInfo);
        var context = new FilterExpressionWorkflowContext
        {
            FilterTermValue = filterValue,
            PropertyMetadata = propertyMetadata,
            Term = term,
        };
        var typeChangingResult = Expression.Constant("bar");
        _typeChangeMock
            .ChangeType(filterValue, propertyType)
            .Returns(typeChangingResult);

        // Act
        _step.Execute(context);

        // Assert
        context.FilterTermConstant.Should().NotBeNull();
        context.FilterTermConstant.Should().Be(typeChangingResult);

        _typeChangeMock
            .Received(1)
            .ChangeType(filterValue, propertyType);
    }

    [Fact]
    public void Should_Covert_When_TypeConverter_CannotConvertFromString()
    {
        // Arrange
        var filterValue = "foo";
        var filterOperator = Substitute.For<IFilterOperator>();
        filterOperator.IsStringBased.Returns(false);
        var term = Substitute.For<IFilterTerm>();
        term.Operator.Returns(filterOperator);
        var propertyType = typeof(int);
        var propertyInfo = Substitute.For<PropertyInfo>();
        propertyInfo.PropertyType.Returns(propertyType);
        var propertyMetadata = Substitute.For<IPropertyMetadata>();
        propertyMetadata.PropertyInfo.Returns(propertyInfo);
        var typeConverter = Substitute.For<ITypeConverter>();
        typeConverter.CanConvertFrom(typeof(string)).Returns(false);
        var context = new FilterExpressionWorkflowContext
        {
            FilterTermValue = filterValue,
            PropertyMetadata = propertyMetadata,
            Term = term,
            TypeConverter = typeConverter,
        };
        var typeChangingResult = Expression.Constant("bar");
        _typeChangeMock
            .ChangeType(filterValue, propertyType)
            .Returns(typeChangingResult);

        // Act
        _step.Execute(context);

        // Assert
        context.FilterTermConstant.Should().NotBeNull();
        context.FilterTermConstant.Should().Be(typeChangingResult);

        _typeChangeMock
            .Received(1)
            .ChangeType(filterValue, propertyType);
    }

    [Fact]
    public void Should_DoNothing_When_CanConvertFromString_AndPropertyTypeIsNotString()
    {
        // Arrange
        var filterOperator = Substitute.For<IFilterOperator>();
        filterOperator.IsStringBased.Returns(false);
        var term = Substitute.For<IFilterTerm>();
        term.Operator.Returns(filterOperator);
        var propertyType = typeof(int);
        var propertyInfo = Substitute.For<PropertyInfo>();
        propertyInfo.PropertyType.Returns(propertyType);
        var propertyMetadata = Substitute.For<IPropertyMetadata>();
        propertyMetadata.PropertyInfo.Returns(propertyInfo);
        var typeConverter = Substitute.For<ITypeConverter>();
        typeConverter.CanConvertFrom(typeof(string)).Returns(true);
        var context = new FilterExpressionWorkflowContext
        {
            PropertyMetadata = propertyMetadata,
            Term = term,
            TypeConverter = typeConverter,
        };

        // Act
        _step.Execute(context);

        // Assert
        context.FilterTermConstant.Should().BeNull();

        _typeChangeMock
            .DidNotReceive()
            .ChangeType(Arg.Any<string>(), Arg.Any<Type>());
    }
}
