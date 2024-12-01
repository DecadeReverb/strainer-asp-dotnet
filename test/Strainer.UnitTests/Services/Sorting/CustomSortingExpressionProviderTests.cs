using Fluorite.Extensions;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Models.Sorting.Terms;
using Fluorite.Strainer.Services.Configuration;
using Fluorite.Strainer.Services.Sorting;
using NSubstitute.ReturnsExtensions;
using System.Linq.Expressions;

namespace Fluorite.Strainer.UnitTests.Services.Sorting;

public class CustomSortingExpressionProviderTests
{
    private readonly IConfigurationCustomMethodsProvider _configurationCustomMethodsProviderMock = Substitute.For<IConfigurationCustomMethodsProvider>();

    private readonly CustomSortingExpressionProvider _provider;

    public CustomSortingExpressionProviderTests()
    {
        _provider = new CustomSortingExpressionProvider(_configurationCustomMethodsProviderMock);
    }

    [Fact]
    public void Should_Return_NoExpression_WhenConfigurationHasNoEntryForModelType()
    {
        // Arrange
        var sortTerm = Substitute.For<ISortTerm>();
        var isSubsequent = false;

        _configurationCustomMethodsProviderMock
            .GetCustomSortMethods()
            .Returns(new Dictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>>().ToReadOnly());

        // Act
        var result = _provider.TryGetCustomExpression<Version>(sortTerm, isSubsequent, out var sortExpression);

        // Assert
        result.Should().BeFalse();
        sortExpression.Should().BeNull();

        _configurationCustomMethodsProviderMock.Received(1).GetCustomSortMethods();
    }

    [Fact]
    public void Should_Return_NoExpression_WhenConfigurationHasNoEntryForSortName()
    {
        // Arrange
        var name = "foo";
        var sortTermMock = Substitute.For<ISortTerm>();
        sortTermMock.Name.Returns(name);
        var isSubsequent = false;
        var customSortMethods = new Dictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>>
        {
            [typeof(Version)] = new Dictionary<string, ICustomSortMethod>().ToReadOnly(),
        }.ToReadOnly();

        _configurationCustomMethodsProviderMock
            .GetCustomSortMethods()
            .Returns(customSortMethods);

        // Act
        var result = _provider.TryGetCustomExpression<Version>(sortTermMock, isSubsequent, out var sortExpression);

        // Assert
        result.Should().BeFalse();
        sortExpression.Should().BeNull();

        _configurationCustomMethodsProviderMock.Received(1).GetCustomSortMethods();
        _ = sortTermMock.Received(1).Name;
    }

    [Fact]
    public void Should_Return_Expression_UsingExpressionProvider()
    {
        // Arrange
        var name = "foo";
        var isDescending = true;
        var isSubsequent = true;
        var sortTermMock = Substitute.For<ISortTerm>();
        sortTermMock.Name.Returns(name);
        sortTermMock.IsDescending.Returns(isDescending);
        Expression<Func<Version, object>> expression = p => p.Major;
        Func<ISortTerm, Expression<Func<Version, object>>> expressionProvider = _ => expression;
        var customSortMethodMock = Substitute.For<ICustomSortMethod<Version>>();
        customSortMethodMock.ExpressionProvider.Returns(expressionProvider);
        customSortMethodMock.Expression.ReturnsNull();
        var modelSortMethods = new Dictionary<string, ICustomSortMethod>
        {
            [name] = customSortMethodMock,
        }.ToReadOnly();
        var customSortMethods = new Dictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>>
        {
            [typeof(Version)] = modelSortMethods,
        }.ToReadOnly();

        _configurationCustomMethodsProviderMock
            .GetCustomSortMethods()
            .Returns(customSortMethods);

        // Act
        var result = _provider.TryGetCustomExpression<Version>(sortTermMock, isSubsequent, out var sortExpression);

        // Assert
        result.Should().BeTrue();
        sortExpression.Should().NotBeNull();
        sortExpression.Expression.Should().BeSameAs(expression);
        sortExpression.IsDefault.Should().BeFalse();
        sortExpression.IsDescending.Should().Be(isDescending);
        sortExpression.IsSubsequent.Should().Be(isSubsequent);

        _configurationCustomMethodsProviderMock.Received(1).GetCustomSortMethods();
        _ = sortTermMock.Received(1).Name;
        _ = customSortMethodMock.Received(2).ExpressionProvider;
        _ = customSortMethodMock.DidNotReceive().Expression;
    }

    [Fact]
    public void Should_Return_Expression_UsingExpression()
    {
        // Arrange
        var name = "foo";
        var isDescending = true;
        var isSubsequent = true;
        var sortTermMock = Substitute.For<ISortTerm>();
        sortTermMock.Name.Returns(name);
        sortTermMock.IsDescending.Returns(isDescending);
        Expression<Func<Version, object>> expression = p => p.Major;
        var customSortMethodMock = Substitute.For<ICustomSortMethod<Version>>();
        customSortMethodMock.ExpressionProvider.ReturnsNull();
        customSortMethodMock.Expression.Returns(expression);
        var modelSortMethods = new Dictionary<string, ICustomSortMethod>
        {
            [name] = customSortMethodMock,
        }.ToReadOnly();
        var customSortMethods = new Dictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>>
        {
            [typeof(Version)] = modelSortMethods,
        }.ToReadOnly();

        _configurationCustomMethodsProviderMock
            .GetCustomSortMethods()
            .Returns(customSortMethods);

        // Act
        var result = _provider.TryGetCustomExpression<Version>(sortTermMock, isSubsequent, out var sortExpression);

        // Assert
        result.Should().BeTrue();
        sortExpression.Should().NotBeNull();
        sortExpression.Expression.Should().BeSameAs(expression);
        sortExpression.IsDefault.Should().BeFalse();
        sortExpression.IsDescending.Should().Be(isDescending);
        sortExpression.IsSubsequent.Should().Be(isSubsequent);

        _configurationCustomMethodsProviderMock.Received(1).GetCustomSortMethods();
        _ = sortTermMock.Received(1).Name;
        _ = customSortMethodMock.Received(1).ExpressionProvider;
        _ = customSortMethodMock.Received(1).Expression;
    }
}
