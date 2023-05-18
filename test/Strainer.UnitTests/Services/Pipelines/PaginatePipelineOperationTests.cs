﻿using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Pagination;
using Fluorite.Strainer.Services.Pipelines;
using Moq;

namespace Fluorite.Strainer.UnitTests.Services.Pipelines
{
    public class PaginatePipelineOperationTests
    {
        private readonly Mock<IPageNumberEvaluator> _pageNumberEvaluatorMock = new();
        private readonly Mock<IPageSizeEvaluator> _pageSizeEvaluatorMock = new();

        private readonly PaginatePipelineOperation _operation;

        public PaginatePipelineOperationTests()
        {
            _operation = new PaginatePipelineOperation(
                _pageNumberEvaluatorMock.Object,
                _pageSizeEvaluatorMock.Object);
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
            var model = Mock.Of<IStrainerModel>();

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
            var model = Mock.Of<IStrainerModel>();

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
            var model = Mock.Of<IStrainerModel>();

            _pageNumberEvaluatorMock
                .Setup(x => x.Evaluate(model))
                .Returns(pageNumber);
            _pageSizeEvaluatorMock
                .Setup(x => x.Evaluate(model))
                .Returns(pageSize);

            // Act
            var result = _operation.Execute(model, source);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.Should().BeEquivalentTo(new [] { 9, 10 });
        }
    }
}
