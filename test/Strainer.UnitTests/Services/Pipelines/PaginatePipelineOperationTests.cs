using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Pagination;
using Fluorite.Strainer.Services.Pipelines;

namespace Fluorite.Strainer.UnitTests.Services.Pipelines;

public class PaginatePipelineOperationTests
{
    private readonly IPageNumberEvaluator _pageNumberEvaluatorMock = Substitute.For<IPageNumberEvaluator>();
    private readonly IPageSizeEvaluator _pageSizeEvaluatorMock = Substitute.For<IPageSizeEvaluator>();

    private readonly PaginatePipelineOperation _operation;

    public PaginatePipelineOperationTests()
    {
        _operation = new PaginatePipelineOperation(
            _pageNumberEvaluatorMock,
            _pageSizeEvaluatorMock);
    }

    [Fact]
    public void Should_Throw_ForNullModel()
    {
        // Arrange
        var source = Enumerable.Empty<object>().AsQueryable();

        // Act
        Action act = () => _operation.Execute(model: null, source);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void Should_Throw_ForNullSource()
    {
        // Arrange
        var model = Substitute.For<IStrainerModel>();

        // Act
        Action act = () => _operation.Execute<object>(model, source: null);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void Should_Return_SourceUnchanged_WhenEvaluatorsReturnDefaultValues()
    {
        // Arrange
        var source = Enumerable.Range(1, 10).AsQueryable();
        var model = Substitute.For<IStrainerModel>();

        // Act
        var result = _operation.Execute(model, source);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(source);
    }

    [Fact]
    public void Should_Return_SourceWithPaginationApplied()
    {
        // Arrange
        var pageNumber = 3;
        var pageSize = 4;
        var source = Enumerable.Range(1, 10).AsQueryable();
        var model = Substitute.For<IStrainerModel>();

        _pageNumberEvaluatorMock
            .Evaluate(model)
            .Returns(pageNumber);
        _pageSizeEvaluatorMock
            .Evaluate(model)
            .Returns(pageSize);

        // Act
        var result = _operation.Execute(model, source);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo([9, 10]);
    }
}
