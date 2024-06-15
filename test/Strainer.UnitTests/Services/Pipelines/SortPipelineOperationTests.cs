using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Models.Sorting.Terms;
using Fluorite.Strainer.Services.Pipelines;
using Fluorite.Strainer.Services.Sorting;
using NSubstitute.ReturnsExtensions;

namespace Fluorite.Strainer.UnitTests.Services.Pipelines;

public class SortPipelineOperationTests
{
    private readonly ISortingApplier _sortingApplierMock = Substitute.For<ISortingApplier>();
    private readonly ISortTermParser _sortTermParserMock = Substitute.For<ISortTermParser>();
    private readonly ISortExpressionProvider _sortExpressionProviderMock = Substitute.For<ISortExpressionProvider>();

    private readonly SortPipelineOperation _operation;

    public SortPipelineOperationTests()
    {
        _operation = new SortPipelineOperation(
            _sortingApplierMock,
            _sortTermParserMock,
            _sortExpressionProviderMock);
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
    public void Should_Return_SortedSource()
    {
        // Arrange
        var source = Enumerable.Range(1, 10).AsQueryable();
        var sortedSource = source.OrderByDescending(x => x).AsQueryable();
        var model = new StrainerModel
        {
            Sorts = "foo",
        };
        var sortTerms = new List<ISortTerm>();

        _sortTermParserMock
            .GetParsedTerms(model.Sorts)
            .Returns(sortTerms);
        _sortingApplierMock
            .TryApplySorting(sortTerms, source, out Arg.Any<IQueryable<int>>())
            .Returns(c =>
            {
                c[2] = sortedSource;

                return true;
            });

        // Act
        var result = _operation.Execute(model, source);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(source);
        result.Should().BeInDescendingOrder();
    }

    [Fact]
    public void Should_Return_SortedSource_WithDefaultSorting()
    {
        // Arrange
        var source = Enumerable.Range(1, 10).AsQueryable();
        IQueryable<int> sortedSource = null;
        var model = new StrainerModel
        {
            Sorts = "foo",
        };
        var sortTerms = new List<ISortTerm>();

        var sortExpressionMock = Substitute.For<ISortExpression<int>>();
        sortExpressionMock
            .IsDescending
            .Returns(true);
        sortExpressionMock
            .Expression
            .Returns(x => x);

        _sortTermParserMock
            .GetParsedTerms(model.Sorts)
            .Returns(sortTerms);
        _sortingApplierMock
            .TryApplySorting(sortTerms, source, out sortedSource)
            .Returns(false);
        _sortExpressionProviderMock
            .GetDefaultExpression<int>()
            .Returns(sortExpressionMock);

        // Act
        var result = _operation.Execute(model, source);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(source);
        result.Should().BeInDescendingOrder();
    }

    [Fact]
    public void Should_Return_UnsortedSource_WhenDefaultSortingIsNotFound()
    {
        // Arrange
        var source = Enumerable.Range(1, 10).AsQueryable();
        var model = new StrainerModel
        {
            Sorts = "foo",
        };
        var sortTerms = new List<ISortTerm>();

        _sortTermParserMock
            .GetParsedTerms(model.Sorts)
            .Returns(sortTerms);
        _sortingApplierMock
            .TryApplySorting(sortTerms, source, out Arg.Any<IQueryable<int>>())
            .Returns(false);
        _sortExpressionProviderMock
            .GetDefaultExpression<int>()
            .ReturnsNull();

        // Act
        var result = _operation.Execute(model, source);

        // Assert
        result.Should().NotBeNull();
        result.Should().NotBeEmpty();
        result.Should().BeEquivalentTo(source);
        result.Should().BeInAscendingOrder();
    }
}
