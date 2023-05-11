using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Models.Sorting.Terms;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Pipelines;
using Fluorite.Strainer.Services.Sorting;
using Moq;

namespace Fluorite.Strainer.UnitTests.Services.Pipelines
{
    public class SortPipelineOperationTests
    {
        private readonly Mock<ISortingApplier> _sortingApplierMock = new();
        private readonly Mock<ISortTermParser> _sortTermParserMock = new();
        private readonly Mock<ISortExpressionProvider> _sortExpressionProviderMock = new();

        private readonly SortPipelineOperation _operation;

        public SortPipelineOperationTests()
        {
            _operation = new SortPipelineOperation(
                _sortingApplierMock.Object,
                _sortTermParserMock.Object,
                _sortExpressionProviderMock.Object);
        }

        [Fact]
        public void Should_Throw_ForNullModel()
        {
            // Arrange
            var source = Enumerable.Empty<object>().AsQueryable();
            var context = Mock.Of<IStrainerContext>();

            // Act
            Action act = () => _operation.Execute(model: null, source, context);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Should_Throw_ForNullSource()
        {
            // Arrange
            var model = Mock.Of<IStrainerModel>();
            var context = Mock.Of<IStrainerContext>();

            // Act
            Action act = () => _operation.Execute<object>(model, source: null, context);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Should_Throw_ForNullContext()
        {
            // Arrange
            var model = Mock.Of<IStrainerModel>();
            var source = Enumerable.Empty<object>().AsQueryable();

            // Act
            Action act = () => _operation.Execute(model, source, context: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Should_Return_SortedSource()
        {
            // Arrange
            var source = Enumerable.Range(1, 10).AsQueryable();
            var sortedSource = source.OrderByDescending(x => x).AsQueryable();
            var context = Mock.Of<IStrainerContext>();
            var model = new StrainerModel
            {
                Sorts = "foo",
            };
            var sortTerms = new List<ISortTerm>();

            _sortTermParserMock
                .Setup(x => x.GetParsedTerms(model.Sorts))
                .Returns(sortTerms);
            _sortingApplierMock
                .Setup(x => x.TryApplySorting(context, sortTerms, source, out sortedSource))
                .Returns(true);

            // Act
            var result = _operation.Execute(model, source, context);

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
            var context = Mock.Of<IStrainerContext>();
            var model = new StrainerModel
            {
                Sorts = "foo",
            };
            var sortTerms = new List<ISortTerm>();

            var sortExpressionMock = new Mock<ISortExpression<int>>();
            sortExpressionMock
                .SetupGet(x => x.IsDescending)
                .Returns(true);
            sortExpressionMock
                .SetupGet(x => x.Expression)
                .Returns(x => x);

            _sortTermParserMock
                .Setup(x => x.GetParsedTerms(model.Sorts))
                .Returns(sortTerms);
            _sortingApplierMock
                .Setup(x => x.TryApplySorting(context, sortTerms, source, out sortedSource))
                .Returns(false);
            _sortExpressionProviderMock
                .Setup(x => x.GetDefaultExpression<int>())
                .Returns(sortExpressionMock.Object);

            // Act
            var result = _operation.Execute(model, source, context);

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
            IQueryable<int> sortedSource = null;
            var context = Mock.Of<IStrainerContext>();
            var model = new StrainerModel
            {
                Sorts = "foo",
            };
            var sortTerms = new List<ISortTerm>();

            _sortTermParserMock
                .Setup(x => x.GetParsedTerms(model.Sorts))
                .Returns(sortTerms);
            _sortingApplierMock
                .Setup(x => x.TryApplySorting(context, sortTerms, source, out sortedSource))
                .Returns(false);

            // Act
            var result = _operation.Execute(model, source, context);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(source);
            result.Should().BeInAscendingOrder();
        }
    }
}
