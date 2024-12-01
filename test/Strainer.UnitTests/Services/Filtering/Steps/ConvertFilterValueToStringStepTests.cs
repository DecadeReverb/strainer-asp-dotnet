using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Filtering.Terms;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Conversion;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Filtering.Steps;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.UnitTests.Services.Filtering.Steps;

public class ConvertFilterValueToStringStepTests
{
    private readonly IStringValueConverter _stringValueConverterMock = Substitute.For<IStringValueConverter>();

    private readonly ConvertFilterValueToStringStep _step;

    public ConvertFilterValueToStringStepTests()
    {
        _step = new ConvertFilterValueToStringStep(_stringValueConverterMock);
    }

    [Fact]
    public void Should_DoNothing_When_FilterTermOperator_IsStringBased()
    {
        // Arrange
        var filterOperator = Substitute.For<IFilterOperator>();
        filterOperator.IsStringBased.Returns(true);
        var term = Substitute.For<IFilterTerm>();
        term.Operator.Returns(filterOperator);
        var propertyInfo = Substitute.For<PropertyInfo>();
        var propertyMetadata = Substitute.For<IPropertyMetadata>();
        propertyMetadata.PropertyInfo.Returns(propertyInfo);
        var typeConverter = Substitute.For<ITypeConverter>();
        var context = new FilterExpressionWorkflowContext
        {
            Term = term,
            PropertyMetadata = propertyMetadata,
            TypeConverter = typeConverter,
            FilterTermValue = "test",
        };

        // Act
        _step.Execute(context);

        // Assert
        _stringValueConverterMock
            .DidNotReceive()
            .Convert(Arg.Any<string>(), Arg.Any<Type>(), Arg.Any<ITypeConverter>());
    }

    [Fact]
    public void Should_DoNothing_When_PropertyType_IsString()
    {
        // Arrange
        var filterOperator = Substitute.For<IFilterOperator>();
        filterOperator.IsStringBased.Returns(false);
        var term = Substitute.For<IFilterTerm>();
        term.Operator.Returns(filterOperator);
        var propertyInfo = Substitute.For<PropertyInfo>();
        propertyInfo.PropertyType.Returns(typeof(string));
        var propertyMetadata = Substitute.For<IPropertyMetadata>();
        propertyMetadata.PropertyInfo.Returns(propertyInfo);
        var typeConverter = Substitute.For<ITypeConverter>();
        var context = new FilterExpressionWorkflowContext
        {
            PropertyMetadata = propertyMetadata,
            Term = term,
            TypeConverter = typeConverter,
            FilterTermValue = "test",
        };

        // Act
        _step.Execute(context);

        // Assert
        _stringValueConverterMock
            .DidNotReceive()
            .Convert(Arg.Any<string>(), Arg.Any<Type>(), Arg.Any<ITypeConverter>());
    }

    [Fact]
    public void Should_DoNothing_When_TypeConverter_CannotConvertFromString()
    {
        // Arrange
        var filterOperator = Substitute.For<IFilterOperator>();
        filterOperator.IsStringBased.Returns(false);
        var term = Substitute.For<IFilterTerm>();
        term.Operator.Returns(filterOperator);
        var propertyInfo = Substitute.For<PropertyInfo>();
        propertyInfo.PropertyType.Returns(typeof(int));
        var propertyMetadata = Substitute.For<IPropertyMetadata>();
        propertyMetadata.PropertyInfo.Returns(propertyInfo);
        var typeConverter = Substitute.For<ITypeConverter>();
        typeConverter.CanConvertFrom(typeof(string)).Returns(false);
        var context = new FilterExpressionWorkflowContext
        {
            PropertyMetadata = propertyMetadata,
            Term = term,
            TypeConverter = typeConverter,
            FilterTermValue = "test",
        };

        // Act
        _step.Execute(context);

        // Assert
        _stringValueConverterMock
            .DidNotReceive()
            .Convert(Arg.Any<string>(), Arg.Any<Type>(), Arg.Any<ITypeConverter>());
    }

    [Fact]
    public void Should_Convert_FilterValueFromString()
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
        typeConverter.CanConvertFrom(typeof(string)).Returns(true);
        var context = new FilterExpressionWorkflowContext
        {
            FilterTermValue = filterValue,
            PropertyMetadata = propertyMetadata,
            Term = term,
            TypeConverter = typeConverter,
        };
        var convertingResult = Expression.Constant(20);
        _stringValueConverterMock
            .Convert(filterValue, propertyType, typeConverter)
            .Returns(convertingResult);

        // Act
        _step.Execute(context);

        // Assert
        context.FilterTermConstant.Should().NotBeNull();
        context.FilterTermConstant.Should().Be(convertingResult);

        _stringValueConverterMock
            .Received(1)
            .Convert(filterValue, propertyType, typeConverter);
    }
}
