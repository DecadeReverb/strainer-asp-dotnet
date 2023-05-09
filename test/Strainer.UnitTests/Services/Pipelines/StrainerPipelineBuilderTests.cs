using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Pipelines;
using Moq;

namespace Fluorite.Strainer.UnitTests.Services.Pipelines
{
    public class StrainerPipelineBuilderTests
    {
        private readonly Mock<IFilterPipelineOperation> _filterPipelineOperationMock = new();
        private readonly Mock<ISortPipelineOperation> _sortPipelineOperationMock = new();
        private readonly Mock<IPaginatePipelineOperation> _paginatePipelineOperationMock = new();

        private readonly StrainerPipelineBuilder _builder;

        public StrainerPipelineBuilderTests()
        {
            _builder = new StrainerPipelineBuilder(
                _filterPipelineOperationMock.Object,
                _sortPipelineOperationMock.Object,
                _paginatePipelineOperationMock.Object);
        }

        [Fact]
        public void Should_Return_Pipeline()
        {
            // Act
            var result = _builder.Build();

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void Should_Return_PipelineWithAllSteps()
        {
            // Arrange
            var query = new List<string> { "foo" }.AsQueryable();
            var model = Mock.Of<IStrainerModel>();
            var context = Mock.Of<IStrainerContext>();

            _filterPipelineOperationMock
                .Setup(x => x.Execute(model, query, context))
                .Returns(query);
            _sortPipelineOperationMock
                .Setup(x => x.Execute(model, query, context))
                .Returns(query);
            _paginatePipelineOperationMock
                .Setup(x => x.Execute(model, query, context))
                .Returns(query);

            // Act
            var pipeline = _builder
                .Filter()
                .Sort()
                .Paginate()
                .Build();

            // Assert
            pipeline.Should().NotBeNull();

            // Act
            var resultQuery = pipeline.Run(model, query, context);

            // Assert
            resultQuery.Should().NotBeNull();
            resultQuery.Should().BeSameAs(query);
            _filterPipelineOperationMock.Verify(x => x.Execute(model, query, context), Times.Once);
            _sortPipelineOperationMock.Verify(x => x.Execute(model, query, context), Times.Once);
            _paginatePipelineOperationMock.Verify(x => x.Execute(model, query, context), Times.Once);
        }
    }
}
