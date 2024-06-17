using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Pipelines;
using NSubstitute.ExceptionExtensions;

namespace Fluorite.Strainer.UnitTests.Services;

public class StrainerProcessorTests
{
    private readonly IStrainerPipelineBuilderFactory _strainerPipelineBuilderFactoryMock = Substitute.For<IStrainerPipelineBuilderFactory>();
    private readonly IStrainerOptionsProvider _strainerOptionsProviderMock = Substitute.For<IStrainerOptionsProvider>();

    private readonly StrainerProcessor _processor;

    public StrainerProcessorTests()
    {
        _processor = new StrainerProcessor(
            _strainerPipelineBuilderFactoryMock,
            _strainerOptionsProviderMock);
    }

    [Fact]
    public void Processor_Applies_AllProcessingSteps()
    {
        // Arrange
        var model = new StrainerModel();
        var options = new StrainerOptions();
        var source = GetSourceQueryable();
        var processedSource = source.Take(10);
        var pipelineBuilderMock = Substitute.For<IStrainerPipelineBuilder>();
        var pipelineMock = Substitute.For<IStrainerPipeline>();

        _strainerOptionsProviderMock
            .GetStrainerOptions()
            .Returns(options);
        _strainerPipelineBuilderFactoryMock
            .CreateBuilder()
            .Returns(pipelineBuilderMock);
        pipelineBuilderMock
            .Build()
            .Returns(pipelineMock);
        pipelineMock
            .Run(model, source)
            .Returns(processedSource);

        // Act
        var result = _processor.Apply(model, source);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().NotBeEquivalentTo(source);
        result.Should().BeSameAs(processedSource);

        Received.InOrder(() =>
        {
            pipelineBuilderMock.Filter();
            pipelineBuilderMock.Sort();
            pipelineBuilderMock.Paginate();
        });
    }

    [Fact]
    public void Processor_DoesNotThrow_DuringProcessing_WhenExceptionThrowingIsDisabled()
    {
        // Arrange
        var model = new StrainerModel();
        var options = new StrainerOptions
        {
            ThrowExceptions = false,
        };
        var source = GetSourceQueryable();
        var pipelineBuilderMock = Substitute.For<IStrainerPipelineBuilder>();
        var pipelineMock = Substitute.For<IStrainerPipeline>();

        _strainerOptionsProviderMock
            .GetStrainerOptions()
            .Returns(options);
        _strainerPipelineBuilderFactoryMock
            .CreateBuilder()
            .Returns(pipelineBuilderMock);
        pipelineBuilderMock
            .Build()
            .Returns(pipelineMock);
        pipelineMock
            .Run(model, source)
            .Throws<StrainerException>();

        // Act
        var result = _processor.Apply(model, source);

        // Assert
        result.Should().BeSameAs(source);
    }

    [Fact]
    public void Processor_Throws_DuringProcessing_WhenExceptionThrowingIsEnabled()
    {
        // Arrange
        var model = new StrainerModel();
        var options = new StrainerOptions
        {
            ThrowExceptions = true,
        };
        var source = GetSourceQueryable();
        var pipelineBuilderMock = Substitute.For<IStrainerPipelineBuilder>();
        var pipelineMock = Substitute.For<IStrainerPipeline>();

        _strainerOptionsProviderMock
            .GetStrainerOptions()
            .Returns(options);
        _strainerPipelineBuilderFactoryMock
            .CreateBuilder()
            .Returns(pipelineBuilderMock);
        pipelineBuilderMock
            .Build()
            .Returns(pipelineMock);
        pipelineMock
            .Run(model, source)
            .Throws<StrainerException>();

        // Act
        Action act = () => _processor.Apply(model, source);

        // Assert
        act.Should().ThrowExactly<StrainerException>();
    }

    [Fact]
    public void Processor_Applies_JustFiltering()
    {
        // Arrange
        var model = new StrainerModel();
        var options = new StrainerOptions();
        var source = GetSourceQueryable();
        var processedSource = source.Take(10);
        var pipelineBuilderMock = Substitute.For<IStrainerPipelineBuilder>();
        var pipelineMock = Substitute.For<IStrainerPipeline>();

        _strainerOptionsProviderMock
            .GetStrainerOptions()
            .Returns(options);
        _strainerPipelineBuilderFactoryMock
            .CreateBuilder()
            .Returns(pipelineBuilderMock);
        pipelineBuilderMock
            .Filter()
            .Returns(pipelineBuilderMock);
        pipelineBuilderMock
            .Build()
            .Returns(pipelineMock);
        pipelineMock
            .Run(model, source)
            .Returns(processedSource);

        // Act
        var result = _processor.ApplyFiltering(model, source);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().NotBeEquivalentTo(source);
        result.Should().BeSameAs(processedSource);

        pipelineBuilderMock.Received(1).Filter();
        pipelineBuilderMock.DidNotReceive().Sort();
        pipelineBuilderMock.DidNotReceive().Paginate();
    }

    [Fact]
    public void Processor_Applies_JustSorting()
    {
        // Arrange
        var model = new StrainerModel();
        var options = new StrainerOptions();
        var source = GetSourceQueryable();
        var processedSource = source.Take(10);
        var pipelineBuilderMock = Substitute.For<IStrainerPipelineBuilder>();
        var pipelineMock = Substitute.For<IStrainerPipeline>();

        _strainerOptionsProviderMock
            .GetStrainerOptions()
            .Returns(options);
        _strainerPipelineBuilderFactoryMock
            .CreateBuilder()
            .Returns(pipelineBuilderMock);
        pipelineBuilderMock
            .Sort()
            .Returns(pipelineBuilderMock);
        pipelineBuilderMock
            .Build()
            .Returns(pipelineMock);
        pipelineMock
            .Run(model, source)
            .Returns(processedSource);

        // Act
        var result = _processor.ApplySorting(model, source);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().NotBeEquivalentTo(source);
        result.Should().BeSameAs(processedSource);

        pipelineBuilderMock.DidNotReceive().Filter();
        pipelineBuilderMock.Received(1).Sort();
        pipelineBuilderMock.DidNotReceive().Paginate();
    }

    [Fact]
    public void Processor_Applies_JustPagination()
    {
        // Arrange
        var model = new StrainerModel();
        var options = new StrainerOptions();
        var source = GetSourceQueryable();
        var processedSource = source.Take(10);
        var pipelineBuilderMock = Substitute.For<IStrainerPipelineBuilder>();
        var pipelineMock = Substitute.For<IStrainerPipeline>();

        _strainerOptionsProviderMock
            .GetStrainerOptions()
            .Returns(options);
        _strainerPipelineBuilderFactoryMock
            .CreateBuilder()
            .Returns(pipelineBuilderMock);
        pipelineBuilderMock
            .Paginate()
            .Returns(pipelineBuilderMock);
        pipelineBuilderMock
            .Build()
            .Returns(pipelineMock);
        pipelineMock
            .Run(model, source)
            .Returns(processedSource);

        // Act
        var result = _processor.ApplyPagination(model, source);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().NotBeEquivalentTo(source);
        result.Should().BeSameAs(processedSource);

        pipelineBuilderMock.DidNotReceive().Filter();
        pipelineBuilderMock.DidNotReceive().Sort();
        pipelineBuilderMock.Received(1).Paginate();
    }

    private class Post
    {
        public string Title { get; set; }
    }

    private IQueryable<Post> GetSourceQueryable()
    {
        return Enumerable.Range(1, 20)
            .Select(x => new Post
            {
                Title = $"Post {x}",
            })
            .AsQueryable();
    }
}
