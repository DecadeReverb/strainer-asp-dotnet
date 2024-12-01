using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Filtering.Terms;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Filtering.Steps;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.UnitTests.Services.Filtering.Steps;

public class MitigateCaseInsensitivityStepTests
{
    private readonly IStrainerOptionsProvider _optionsProviderMock = Substitute.For<IStrainerOptionsProvider>();
    private readonly MitigateCaseInsensitivityStep _step;

    public MitigateCaseInsensitivityStepTests()
    {
        _step = new MitigateCaseInsensitivityStep(_optionsProviderMock);
    }

    [Fact]
    public void Should_DoNothing_WhenPropertyIsNotStringType()
    {
        // Arrange
        var type = typeof(Version);
        var propertyInfoMock = Substitute.For<PropertyInfo>();
        propertyInfoMock
            .PropertyType
            .Returns(type);
        var metadataMock = Substitute.For<IPropertyMetadata>();
        metadataMock
            .PropertyInfo
            .Returns(propertyInfoMock);
        var filterOperatorMock = Substitute.For<IFilterOperator>();
        var filterTermMock = Substitute.For<IFilterTerm>();
        filterTermMock
            .Operator
            .Returns(filterOperatorMock);
        var context = new FilterExpressionWorkflowContext
        {
            PropertyMetadata = metadataMock,
            Term = filterTermMock,
        };
        var options = new StrainerOptions();

        _optionsProviderMock
            .GetStrainerOptions()
            .Returns(options);

        // Act
        _step.Execute(context);

        // Assert
        context.PropertyValue.Should().BeNull();
        context.FinalExpression.Should().BeNull();
    }

    [Fact]
    public void Should_CallToUpper_WhenOperatorIsCaseInsensitive()
    {
        // Arrange
        var type = typeof(string);
        var propertyInfoMock = Substitute.For<PropertyInfo>();
        propertyInfoMock
            .PropertyType
            .Returns(type);
        var metadataMock = Substitute.For<IPropertyMetadata>();
        metadataMock
            .PropertyInfo
            .Returns(propertyInfoMock);
        var filterOperatorMock = Substitute.For<IFilterOperator>();
        filterOperatorMock
            .IsCaseInsensitive
            .Returns(true);
        var filterTermMock = Substitute.For<IFilterTerm>();
        filterTermMock
            .Operator
            .Returns(filterOperatorMock);
        Expression<Func<Blog, string>> expression = b => b.Title;
        var context = new FilterExpressionWorkflowContext
        {
            PropertyMetadata = metadataMock,
            Term = filterTermMock,
            PropertyValue = Expression.Constant("foo"),
            FinalExpression = expression.Body,
        };
        var options = new StrainerOptions();

        _optionsProviderMock
            .GetStrainerOptions()
            .Returns(options);

        // Act
        _step.Execute(context);

        // Assert
        context.PropertyValue.Should().BeAssignableTo<MethodCallExpression>();
        context.FinalExpression.Should().BeAssignableTo<MethodCallExpression>();
    }

    [Fact]
    public void Should_CallToUpper_WhenOperatorIsNotCaseInsensitive_ButOptionsHaveEnabledCaseInsensivityForValues()
    {
        // Arrange
        var type = typeof(string);
        var propertyInfoMock = Substitute.For<PropertyInfo>();
        propertyInfoMock
            .PropertyType
            .Returns(type);
        var metadataMock = Substitute.For<IPropertyMetadata>();
        metadataMock
            .PropertyInfo
            .Returns(propertyInfoMock);
        var filterOperatorMock = Substitute.For<IFilterOperator>();
        var filterTermMock = Substitute.For<IFilterTerm>();
        filterTermMock
            .Operator
            .Returns(filterOperatorMock);
        Expression<Func<Blog, string>> expression = b => b.Title;
        var context = new FilterExpressionWorkflowContext
        {
            PropertyMetadata = metadataMock,
            Term = filterTermMock,
            PropertyValue = Expression.Constant("foo"),
            FinalExpression = expression.Body,
        };
        var options = new StrainerOptions
        {
            IsCaseInsensitiveForValues = true,
        };

        _optionsProviderMock
            .GetStrainerOptions()
            .Returns(options);

        // Act
        _step.Execute(context);

        // Assert
        context.PropertyValue.Should().BeAssignableTo<MethodCallExpression>();
        context.FinalExpression.Should().BeAssignableTo<MethodCallExpression>();
    }

    private class Blog
    {
        public string Title { get; set; }
    }
}
