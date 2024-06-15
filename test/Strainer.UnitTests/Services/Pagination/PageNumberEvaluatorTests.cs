using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Pagination;

namespace Fluorite.Strainer.UnitTests.Services.Pagination;

public class PageNumberEvaluatorTests
{
    private readonly IStrainerOptionsProvider _strainerOptionsProviderMock = Substitute.For<IStrainerOptionsProvider>();

    private readonly PageNumberEvaluator _evaluator;

    public PageNumberEvaluatorTests()
    {
        _evaluator = new PageNumberEvaluator(_strainerOptionsProviderMock);
    }

    [Fact]
    public void Should_Throw_ForNullOptionsProvider()
    {
        // Arrange & Act
        Action act = () => new PageNumberEvaluator(strainerOptionsProvider: null);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void Should_Throw_ForNullStrainerModel()
    {
        // Arrange & Act
        Action act = () => _evaluator.Evaluate(model: null);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Theory]
    [InlineData(0)]
    [InlineData(23)]
    [InlineData(int.MaxValue)]
    public void Should_Return_PageNumber(int? pageNumber)
    {
        // Arrange
        var model = new StrainerModel
        {
            Page = pageNumber,
        };

        // Act
        var result = _evaluator.Evaluate(model);

        // Assert
        result.Should().Be(pageNumber.Value);
    }

    [Fact]
    public void Should_Return_DefaultPageNumber_WhenModelPageNumberIsNull()
    {
        // Arrange
        var model = new StrainerModel
        {
            Page = null,
        };
        var defaultPageNumber = 23;
        var strainerOptions = new StrainerOptions
        {
            DefaultPageNumber = defaultPageNumber,
        };

        _strainerOptionsProviderMock
            .GetStrainerOptions()
            .Returns(strainerOptions);

        // Act
        var result = _evaluator.Evaluate(model);

        // Assert
        result.Should().Be(defaultPageNumber);
    }
}
