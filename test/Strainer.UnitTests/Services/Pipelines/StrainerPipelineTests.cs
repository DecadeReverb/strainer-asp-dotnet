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
            // Act
            Action action = () => _ = new StrainerPipeline(operations: null);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_Throw_ForNullModel()
        {
            // Assert
            var operations = new List<IStrainerPipelineOperation>();
            var pipeline = new StrainerPipeline(operations);
            var source = new List<Uri>().AsQueryable();
            var context = Mock.Of<IStrainerContext>();

            // Act
            Action action = () => pipeline.Run(model: null, source, context);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_Throw_ForNullSource()
        {
            // Assert
            var operations = new List<IStrainerPipelineOperation>();
            var pipeline = new StrainerPipeline(operations);
            var model = new StrainerModel();
            var context = Mock.Of<IStrainerContext>();

            // Act
            Action action = () => pipeline.Run<Uri>(model, source: null, context);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_Throw_ForNullContext()
        {
            // Assert
            var operations = new List<IStrainerPipelineOperation>();
            var pipeline = new StrainerPipeline(operations);
            var model = new StrainerModel();
            var source = new List<Uri>().AsQueryable();

            // Act
            Action action = () => pipeline.Run(model, source, context: null);

            // Assert
            action.Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void Should_Execute_Operations_Empty()
        {
            // Assert
            var operations = new List<IStrainerPipelineOperation>();
            var pipeline = new StrainerPipeline(operations);
            var model = new StrainerModel();
            var source = new List<Uri>().AsQueryable();
            var context = Mock.Of<IStrainerContext>();

            // Act
            var result = pipeline.Run(model, source, context);

            // Assert
            result.Should().BeSameAs(source);
        }

        [Fact]
        public void Should_Execute_Operations()
        {
            // Assert
            var operationResult = new List<Uri>().AsQueryable().Where(x => x.IsFile);
            var operationMock = new Mock<IStrainerPipelineOperation>();
            operationMock
                .Setup(x => x.Execute(
                    It.IsAny<IStrainerModel>(),
                    It.IsAny<IQueryable<Uri>>(),
                    It.IsAny<IStrainerContext>()))
                .Returns(operationResult);
            var operations = new List<IStrainerPipelineOperation>
            {
                operationMock.Object,
            };
            var pipeline = new StrainerPipeline(operations);
            var model = new StrainerModel();
            var source = new List<Uri>().AsQueryable();
            var context = Mock.Of<IStrainerContext>();

            // Act
            var result = pipeline.Run(model, source, context);

            // Assert
            result.Should().BeSameAs(operationResult);
        }

        [Fact]
        public void Should_Return_Source_WhenOperationThrows()
        {
            // Assert
            var operationMock = new Mock<IStrainerPipelineOperation>();
            operationMock
                .Setup(x => x.Execute(
                    It.IsAny<IStrainerModel>(),
                    It.IsAny<IQueryable<Uri>>(),
                    It.IsAny<IStrainerContext>()))
                .Throws(new StrainerException());
            var operations = new List<IStrainerPipelineOperation>
            {
                operationMock.Object,
            };
            var pipeline = new StrainerPipeline(operations);
            var model = new StrainerModel();
            var source = new List<Uri>().AsQueryable();
            var contextMock = new Mock<IStrainerContext>();
            contextMock
                .Setup(x => x.Options)
                .Returns(new StrainerOptions());

            // Act
            var result = pipeline.Run(model, source, contextMock.Object);

            // Assert
            result.Should().BeSameAs(source);
        }

        [Fact]
        public void Should_PassException_WhenOperationThrows_WithEnabledExceptions()
        {
            // Assert
            var operationMock = new Mock<IStrainerPipelineOperation>();
            operationMock
                .Setup(x => x.Execute(
                    It.IsAny<IStrainerModel>(),
                    It.IsAny<IQueryable<Uri>>(),
                    It.IsAny<IStrainerContext>()))
                .Throws(new StrainerException());
            var operations = new List<IStrainerPipelineOperation>
            {
                operationMock.Object,
            };
            var pipeline = new StrainerPipeline(operations);
            var model = new StrainerModel();
            var source = new List<Uri>().AsQueryable();
            var contextMock = new Mock<IStrainerContext>();
            contextMock
                .Setup(x => x.Options)
                .Returns(new StrainerOptions
                {
                    ThrowExceptions = true,
                });

            // Act
            Action act = () => pipeline.Run(model, source, contextMock.Object);

            // Assert
            act.Should().Throw<StrainerException>();
        }
    }
}
