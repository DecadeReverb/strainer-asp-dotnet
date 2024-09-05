using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Terms;
using Fluorite.Strainer.Services.Configuration;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Extensions;
using System.Linq.Expressions;
using NSubstitute.ReturnsExtensions;

namespace Fluorite.Strainer.UnitTests.Services.Filtering;

public class CustomFilteringExpressionProviderTests
{
    private readonly IConfigurationCustomMethodsProvider _configurationCustomMethodsProviderMock = Substitute.For<IConfigurationCustomMethodsProvider>();

    private readonly CustomFilteringExpressionProvider _provider;

    public CustomFilteringExpressionProviderTests()
    {
        _provider = new CustomFilteringExpressionProvider(_configurationCustomMethodsProviderMock);
    }

    [Fact]
    public void Should_Return_NoExpression_WhenFilterMethodsAreEmpty()
    {
        // Arrange
        var filterTermName = nameof(Post.Name);
        var filterTermMock = Substitute.For<IFilterTerm>();
        var customFilterMethods = new Dictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>().ToReadOnly();

        _configurationCustomMethodsProviderMock
            .GetCustomFilterMethods()
            .Returns(customFilterMethods);

        // Act
        var result = _provider.TryGetCustomExpression<Post>(filterTermMock, filterTermName, out var expression);

        // Assert
        result.Should().BeFalse();
        expression.Should().BeNull();
    }

    [Fact]
    public void Should_Return_NoExpression_WhenTypeMethodsAreEmpty()
    {
        // Arrange
        var filterTermName = nameof(Post.Name);
        var filterTermMock = Substitute.For<IFilterTerm>();
        var customFilterMethods = new Dictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>
        {
            [typeof(Post)] = new Dictionary<string, ICustomFilterMethod>().ToReadOnly(),
        };

        _configurationCustomMethodsProviderMock
            .GetCustomFilterMethods()
            .Returns(customFilterMethods.ToReadOnly());

        // Act
        var result = _provider.TryGetCustomExpression<Post>(filterTermMock, filterTermName, out var expression);

        // Assert
        result.Should().BeFalse();
        expression.Should().BeNull();
    }

    [Fact]
    public void Should_Return_DirectExpression()
    {
        // Arrange
        var filterTermName = nameof(Post.Name);
        var filterTermMock = Substitute.For<IFilterTerm>();
        Expression<Func<Post, bool>> directExpression = x => true;
        var customFilterMethodMock = Substitute.For<ICustomFilterMethod<Post>>();
        var typeFilterMethods = new Dictionary<string, ICustomFilterMethod>
        {
            [nameof(Post.Name)] = customFilterMethodMock,
        };
        var customFilterMethods = new Dictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>
        {
            [typeof(Post)] = typeFilterMethods.ToReadOnly(),
        };

        customFilterMethodMock
            .Expression
            .Returns(directExpression);
        customFilterMethodMock
            .FilterTermExpression
            .ReturnsNull();
        _configurationCustomMethodsProviderMock
            .GetCustomFilterMethods()
            .Returns(customFilterMethods.ToReadOnly());

        // Act
        var result = _provider.TryGetCustomExpression<Post>(filterTermMock, filterTermName, out var expression);

        // Assert
        result.Should().BeTrue();
        expression.Should().NotBeNull();
        expression.Should().BeSameAs(directExpression);
    }

    [Fact]
    public void Should_Return_MethodExpression()
    {
        // Arrange
        var filterTermName = nameof(Post.Name);
        var filterTermMock = Substitute.For<IFilterTerm>();
        Expression<Func<Post, bool>> methodExpression = x => true;
        var customFilterMethodMock = Substitute.For<ICustomFilterMethod<Post>>();
        var typeFilterMethods = new Dictionary<string, ICustomFilterMethod>
        {
            [nameof(Post.Name)] = customFilterMethodMock,
        };
        var customFilterMethods = new Dictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>
        {
            [typeof(Post)] = typeFilterMethods.ToReadOnly(),
        };

        customFilterMethodMock
            .Expression
            .ReturnsNull();
        customFilterMethodMock
            .FilterTermExpression
            .Returns(x => methodExpression);
        _configurationCustomMethodsProviderMock
            .GetCustomFilterMethods()
            .Returns(customFilterMethods.ToReadOnly());

        // Act
        var result = _provider.TryGetCustomExpression<Post>(filterTermMock, filterTermName, out var expression);

        // Assert
        result.Should().BeTrue();
        expression.Should().NotBeNull();
        expression.Should().BeSameAs(methodExpression);
    }

    internal class Post
    {
        public string Name { get; set; }
    }
}
