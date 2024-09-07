using Fluorite.Strainer.Models.Filtering.Terms;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Conversion;
using Fluorite.Strainer.Services.Filtering;
using NSubstitute.ReturnsExtensions;
using System.Linq.Expressions;
using System.Reflection;

namespace Fluorite.Strainer.UnitTests.Services.Filtering;

public class FilterExpressionProviderTests
{
    private readonly ITypeConverterProvider _typeConverterProviderMock = Substitute.For<ITypeConverterProvider>();
    private readonly IFilterExpressionWorkflowBuilder _filterExpressionWorkflowBuilderMock = Substitute.For<IFilterExpressionWorkflowBuilder>();

    private readonly FilterExpressionProvider _provider;

    public FilterExpressionProviderTests()
    {
        _provider = new FilterExpressionProvider(
            _typeConverterProviderMock,
            _filterExpressionWorkflowBuilderMock);
    }

    [Fact]
    public void Should_Return_Null_WhenFilterTermValuesAreNull()
    {
        // Arrange
        var metadata = Substitute.For<IPropertyMetadata>();
        var filterTerm = Substitute.For<IFilterTerm>();
        var parameterExpression = Expression.Parameter(typeof(Post), "p");

        filterTerm.Values.ReturnsNull();

        // Act
        var result = _provider.GetExpression(metadata, filterTerm, parameterExpression, innerExpression: null);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Should_Return_Null_WhenFilterTermValuesAreEmpty()
    {
        // Arrange
        var metadata = Substitute.For<IPropertyMetadata>();
        var filterTerm = Substitute.For<IFilterTerm>();
        var parameterExpression = Expression.Parameter(typeof(Post), "p");

        filterTerm.Values.Returns(new List<string>());

        // Act
        var result = _provider.GetExpression(metadata, filterTerm, parameterExpression, innerExpression: null);

        // Assert
        result.Should().BeNull();
    }

    [Theory]
    [InlineData("")]
    [InlineData(".")]
    [InlineData("...")]
    public void Should_Throw_WhenMetadataNameIsEmpty(string name)
    {
        // Arrange
        var metadata = Substitute.For<IPropertyMetadata>();
        var filterTerm = Substitute.For<IFilterTerm>();
        var parameterExpression = Expression.Parameter(typeof(Post), "p");

        filterTerm.Values.Returns(new[] { "foo"});
        metadata.Name.Returns(name);

        // Act
        Action act = () => _provider.GetExpression(metadata, filterTerm, parameterExpression, innerExpression: null);

        // Assert
        act.Should().ThrowExactly<ArgumentException>()
            .WithMessage("Metadata name must not be empty.*");
    }

    [Fact]
    public void Should_Return_Expression()
    {
        // Arrange
        var metadata = Substitute.For<IPropertyMetadata>();
        var propertyInfoMock = Substitute.For<PropertyInfo>();
        var typeConverterMock = Substitute.For<ITypeConverter>();
        var filterExpressionWorkflowMock = Substitute.For<IFilterExpressionWorkflow>();
        var filterTerm = Substitute.For<IFilterTerm>();
        var modelType = typeof(Post);
        var parameterExpression = Expression.Parameter(modelType, "p");
        var expression = Expression.Constant(true);
        var value = "foo";
        var name = nameof(Post.Title);

        filterTerm.Values.Returns(new List<string> { value});
        propertyInfoMock.PropertyType.Returns(modelType);
        metadata.Name.Returns(name);
        metadata.PropertyInfo.Returns(propertyInfoMock);
        _typeConverterProviderMock
            .GetTypeConverter(modelType)
            .Returns(typeConverterMock);
        _filterExpressionWorkflowBuilderMock
            .BuildDefaultWorkflow()
            .Returns(filterExpressionWorkflowMock);
        filterExpressionWorkflowMock
            .Run(Arg.Is<FilterExpressionWorkflowContext>(x =>
                x.FilterTermConstant.Equals(value)
                && x.FilterTermValue == value
                && x.FinalExpression == null
                && x.PropertyMetadata.Equals(metadata)
                && x.Term == filterTerm
                && x.TypeConverter.Equals(typeConverterMock)))
            .Returns(expression);

        // Act
        var result = _provider.GetExpression(metadata, filterTerm, parameterExpression, innerExpression: null);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(expression);
    }

    [Fact]
    public void Should_Return_ExpressionJoinedByOrWithInnerExpression()
    {
        // Arrange
        var metadata = Substitute.For<IPropertyMetadata>();
        var propertyInfoMock = Substitute.For<PropertyInfo>();
        var typeConverterMock = Substitute.For<ITypeConverter>();
        var filterExpressionWorkflowMock = Substitute.For<IFilterExpressionWorkflow>();
        var filterTerm = Substitute.For<IFilterTerm>();
        var modelType = typeof(Post);
        var parameterExpression = Expression.Parameter(modelType, "p");
        var expression = Expression.Constant(true);
        var innerExpression = Expression.Constant(false);
        var value = "foo";
        var name = nameof(Post.Title);

        filterTerm.Values.Returns(new List<string> { value });
        propertyInfoMock.PropertyType.Returns(modelType);
        metadata.Name.Returns(name);
        metadata.PropertyInfo.Returns(propertyInfoMock);
        _typeConverterProviderMock
            .GetTypeConverter(modelType)
            .Returns(typeConverterMock);
        _filterExpressionWorkflowBuilderMock
            .BuildDefaultWorkflow()
            .Returns(filterExpressionWorkflowMock);
        filterExpressionWorkflowMock
            .Run(Arg.Is<FilterExpressionWorkflowContext>(x =>
                x.FilterTermConstant.Equals(value)
                && x.FilterTermValue == value
                && x.FinalExpression == null
                && x.PropertyMetadata.Equals(metadata)
                && x.Term == filterTerm
                && x.TypeConverter.Equals(typeConverterMock)))
            .Returns(expression);

        // Act
        var result = _provider.GetExpression(metadata, filterTerm, parameterExpression, innerExpression);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeOfType<BinaryExpression>();
        result.Should().BeEquivalentTo(Expression.Or(innerExpression, expression));
    }

    private class Post
    {
        public string Title { get; set; }
    }
}
