using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Pipelines;
using Moq;

namespace Fluorite.Strainer.UnitTests.Services.Pipelines
{
    public class StrainerPipelineTests
    {
        [Fact]
        public void Should_Throw_ForNullOperations()
        {
            // Arrange
            var strainerOptionsProvider = Mock.Of<IStrainerOptionsProvider>();

            // Act
            Action action = () => _ = new StrainerPipeline(operations: null, strainerOptionsProvider);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Should_Throw_ForNullOptionsProvider()
        {
            // Arrange
            var operations = new List<IStrainerPipelineOperation>();

            // Act
            Action action = () => _ = new StrainerPipeline(operations, strainerOptionsProvider: null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Should_Throw_ForNullModel()
        {
            // Arrange
            var strainerOptionsProvider = Mock.Of<IStrainerOptionsProvider>();
            var operations = new List<IStrainerPipelineOperation>();
            var pipeline = CreatePipeline(operations, strainerOptionsProvider);
            var source = new List<Uri>().AsQueryable();

            // Act
            Action action = () => pipeline.Run(model: null, source);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Should_Throw_ForNullSource()
        {
            // Arrange
            var strainerOptionsProvider = Mock.Of<IStrainerOptionsProvider>();
            var operations = new List<IStrainerPipelineOperation>();
            var pipeline = CreatePipeline(operations, strainerOptionsProvider);
            var model = new StrainerModel();

            // Act
            Action action = () => pipeline.Run<Uri>(model, source: null);

            // Assert
            action.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Should_Execute_Operations_Empty()
        {
            // Arrange
            var strainerOptionsProvider = Mock.Of<IStrainerOptionsProvider>();
            var operations = new List<IStrainerPipelineOperation>();
            var pipeline = CreatePipeline(operations, strainerOptionsProvider);
            var model = new StrainerModel();
            var source = new List<Uri>().AsQueryable();

            // Act
            var result = pipeline.Run(model, source);

            // Assert
            result.Should().BeSameAs(source);
        }

        [Fact]
        public void Should_Execute_Operations()
        {
            // Arrange
            var strainerOptionsProvider = Mock.Of<IStrainerOptionsProvider>();
            var operationResult = new List<Uri>().AsQueryable().Where(x => x.IsFile);
            var operationMock = new Mock<IStrainerPipelineOperation>();
            operationMock
                .Setup(x => x.Execute(
                    It.IsAny<IStrainerModel>(),
                    It.IsAny<IQueryable<Uri>>()))
                .Returns(operationResult);
            var operations = new List<IStrainerPipelineOperation>
            {
                operationMock.Object,
            };
            var pipeline = CreatePipeline(operations, strainerOptionsProvider);
            var model = new StrainerModel();
            var source = new List<Uri>().AsQueryable();
            var context = Mock.Of<IStrainerContext>();

            // Act
            var result = pipeline.Run(model, source);

            // Assert
            result.Should().BeSameAs(operationResult);
        }

        [Fact]
        public void Should_Return_Source_WhenOperationThrows()
        {
            // Arrange
            var strainerOptionsProviderMock = new Mock<IStrainerOptionsProvider>();
            strainerOptionsProviderMock
                .Setup(x => x.GetStrainerOptions())
                .Returns(new StrainerOptions());

            var operationMock = new Mock<IStrainerPipelineOperation>();
            operationMock
                .Setup(x => x.Execute(
                    It.IsAny<IStrainerModel>(),
                    It.IsAny<IQueryable<Uri>>()))
                .Throws(new StrainerException());
            var operations = new List<IStrainerPipelineOperation>
            {
                operationMock.Object,
            };
            var pipeline = CreatePipeline(operations, strainerOptionsProviderMock.Object);
            var model = new StrainerModel();
            var source = new List<Uri>().AsQueryable();

            // Act
            var result = pipeline.Run(model, source);

            // Assert
            result.Should().BeSameAs(source);
        }

        [Fact]
        public void Should_PassException_WhenOperationThrows_WithEnabledExceptions()
        {
            // Arrange
            var strainerOptionsProviderMock = new Mock<IStrainerOptionsProvider>();
            strainerOptionsProviderMock
                .Setup(x => x.GetStrainerOptions())
                .Returns(new StrainerOptions
                {
                    ThrowExceptions = true,
                });

            var operationMock = new Mock<IStrainerPipelineOperation>();
            operationMock
                .Setup(x => x.Execute(
                    It.IsAny<IStrainerModel>(),
                    It.IsAny<IQueryable<Uri>>()))
                .Throws(new StrainerException());
            var operations = new List<IStrainerPipelineOperation>
            {
                operationMock.Object,
            };
            var pipeline = CreatePipeline(operations, strainerOptionsProviderMock.Object);
            var model = new StrainerModel();
            var source = new List<Uri>().AsQueryable();

            // Act
            Action act = () => pipeline.Run(model, source);

            // Assert
            act.Should().ThrowExactly<StrainerException>();
        }

        private static StrainerPipeline CreatePipeline(
            IEnumerable<IStrainerPipelineOperation> operations,
            IStrainerOptionsProvider strainerOptionsProvider)
        {
            return new StrainerPipeline(operations, strainerOptionsProvider);
        }
    }
}
