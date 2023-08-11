using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Pipelines;

namespace Fluorite.Strainer.UnitTests.Services.Pipelines
{
    public class StrainerPipelineBuilderTests
    {
        private readonly IFilterPipelineOperation _filterPipelineOperationMock = Substitute.For<IFilterPipelineOperation>();
        private readonly ISortPipelineOperation _sortPipelineOperationMock = Substitute.For<ISortPipelineOperation>();
        private readonly IPaginatePipelineOperation _paginatePipelineOperationMock = Substitute.For<IPaginatePipelineOperation>();
        private readonly IStrainerOptionsProvider _strainerOptionsProviderMock = Substitute.For<IStrainerOptionsProvider>();

        private readonly StrainerPipelineBuilder _builder;

        public StrainerPipelineBuilderTests()
        {
            _builder = new StrainerPipelineBuilder(
                _filterPipelineOperationMock,
                _sortPipelineOperationMock,
                _paginatePipelineOperationMock,
                _strainerOptionsProviderMock);
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
            var model = Substitute.For<IStrainerModel>();

            _filterPipelineOperationMock
                .Execute(model, query)
                .Returns(query);
            _sortPipelineOperationMock
                .Execute(model, query)
                .Returns(query);
            _paginatePipelineOperationMock
                .Execute(model, query)
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
            var resultQuery = pipeline.Run(model, query);

            // Assert
            resultQuery.Should().NotBeNull();
            resultQuery.Should().BeSameAs(query);

            _filterPipelineOperationMock.Received(1).Execute(model, query);
            _sortPipelineOperationMock.Received(1).Execute(model, query);
            _paginatePipelineOperationMock.Received(1).Execute(model, query);
        }
    }
}
