using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Filtering.Terms;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Filtering.Steps;
using NSubstitute.ExceptionExtensions;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.UnitTests.Services.Filtering.Steps;

public class ApplyFilterOperatorStepTests
{
    private readonly ApplyFilterOperatorStep _step;

    public ApplyFilterOperatorStepTests()
    {
        _step = new ApplyFilterOperatorStep();
    }

    [Fact]
    public void Should_Invoke_FilterOperatorExpression()
    {
        // Arrange
        var context = new FilterExpressionWorkflowContext
        {
            FinalExpression = Expression.Constant("foo"),
            PropertyValue = Expression.Constant("bar"),
            FilterTermValue = "test",
        };
        var finalExpression = Expression.Constant("lorem ipsum");
        var funcMock = Substitute.For<Func<IFilterExpressionContext, Expression>>();
        funcMock
            .Invoke(Arg.Is<FilterExpressionContext>(
                x => x.FilterValue.Equals(context.FinalExpression)
                && x.PropertyValue.Equals(context.PropertyValue)))
            .Returns(finalExpression);
        var filterOperator = Substitute.For<IFilterOperator>();
        filterOperator
            .Expression
            .Returns(funcMock);
        var filterTerm = Substitute.For<IFilterTerm>();
        filterTerm
            .Operator
            .Returns(filterOperator);
        context.Term = filterTerm;
        var propertyInfo = Substitute.For<PropertyInfo>();
        var propertyMetadata = Substitute.For<IPropertyMetadata>();
        propertyMetadata
            .PropertyInfo
            .Returns(propertyInfo);
        context.PropertyMetadata = propertyMetadata;

        // Act
        _step.Execute(context);

        // Assert
        context.FinalExpression.Should().Be(finalExpression);

        funcMock.Received(1);
    }

    [Fact]
    public void Should_Throw()
    {
        // Arrange
        var innerException = new InvalidOperationException();
        var func = Substitute.For<Func<IFilterExpressionContext, Expression>>();
        func
            .Invoke(Arg.Any<IFilterExpressionContext>())
            .Throws(innerException);
        var filterOperator = Substitute.For<IFilterOperator>();
        filterOperator
            .Expression
            .Returns(func);
        var filterTerm = Substitute.For<IFilterTerm>();
        filterTerm
            .Operator
            .Returns(filterOperator);
        var propertyMetadata = Substitute.For<IPropertyMetadata>();
        propertyMetadata
            .PropertyInfo
            .Returns(typeof(Blog).GetProperty(nameof(Blog.Title)));
        var context = new FilterExpressionWorkflowContext
        {
            FilterTermValue = "lorem",
            FinalExpression = Expression.Constant("foo"),
            PropertyMetadata = propertyMetadata,
            PropertyValue = Expression.Constant("bar"),
            Term = filterTerm,
        };

        // Act
        Action act = () => _step.Execute(context);

        // Assert
        act.Should().ThrowExactly<StrainerOperatorException>()
            .WithMessage(
                $"Failed to use operator * for filter value '{context.FilterTermValue}' on property '*Blog.Title' of type 'System.String'\n." +
                "Please ensure that this operator is supported by type 'System.String'.")
            .WithInnerExceptionExactly(innerException.GetType());
    }

    private class Blog
    {
        public string Title { get; set; }
    }
}
