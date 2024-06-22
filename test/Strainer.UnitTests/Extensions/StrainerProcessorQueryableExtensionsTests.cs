using Fluorite.Extensions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;

namespace Fluorite.Strainer.UnitTests.Extensions;

public class StrainerProcessorQueryableExtensionsTests
{
    [Fact]
    public void Apply_Should_Return_Source_WhenModelIsNull()
    {
        // Arrange
        var value = "foo";
        var source = new string[] { value }.AsQueryable();
        IStrainerModel strainerModel = null;
        var strainerProcessor = Substitute.For<IStrainerProcessor>();

        // Act
        source = source.Apply(strainerModel, strainerProcessor);

        // Assert
        source.Should().BeEquivalentTo(value);

        strainerProcessor.DidNotReceive().Apply(strainerModel, source, true, true, true);
    }

    [Fact]
    public void Apply_Should_CallProcessor()
    {
        // Arrange
        var value = "foo";
        var source = new string[] { value }.AsQueryable();
        var strainerModel = Substitute.For<IStrainerModel>();
        var strainerProcessor = Substitute.For<IStrainerProcessor>();

        strainerProcessor
            .Apply(strainerModel, source, true, true, true)
            .Returns(Array.Empty<string>().AsQueryable());

        // Act
        var result = source.Apply(strainerModel, strainerProcessor);

        // Assert
        result.Should().BeEmpty();

        strainerProcessor.Received(1).Apply(strainerModel, source, true, true, true);
    }

    [Fact]
    public void ApplyFiltering_Should_Return_Source_WhenModelIsNull()
    {
        // Arrange
        var value = "foo";
        var source = new string[] { value }.AsQueryable();
        IStrainerModel strainerModel = null;
        var strainerProcessor = Substitute.For<IStrainerProcessor>();

        // Act
        source = source.ApplyFiltering(strainerModel, strainerProcessor);

        // Assert
        source.Should().BeEquivalentTo(value);

        strainerProcessor.DidNotReceive().ApplyFiltering(strainerModel, source);
    }

    [Fact]
    public void ApplyFiltering_Should_CallProcessor()
    {
        // Arrange
        var value = "foo";
        var source = new string[] { value }.AsQueryable();
        var strainerModel = Substitute.For<IStrainerModel>();
        var strainerProcessor = Substitute.For<IStrainerProcessor>();

        strainerProcessor
            .ApplyFiltering(strainerModel, source)
            .Returns(Array.Empty<string>().AsQueryable());

        // Act
        var result = source.ApplyFiltering(strainerModel, strainerProcessor);

        // Assert
        result.Should().BeEmpty();

        strainerProcessor.Received(1).ApplyFiltering(strainerModel, source);
    }

    [Fact]
    public void ApplyPagination_Should_Return_Source_WhenModelIsNull()
    {
        // Arrange
        var value = "foo";
        var source = new string[] { value }.AsQueryable();
        IStrainerModel strainerModel = null;
        var strainerProcessor = Substitute.For<IStrainerProcessor>();

        // Act
        source = source.ApplyPagination(strainerModel, strainerProcessor);

        // Assert
        source.Should().BeEquivalentTo(value);

        strainerProcessor.DidNotReceive().ApplyPagination(strainerModel, source);
    }

    [Fact]
    public void ApplyPagination_Should_CallProcessor()
    {
        // Arrange
        var value = "foo";
        var source = new string[] { value }.AsQueryable();
        var strainerModel = Substitute.For<IStrainerModel>();
        var strainerProcessor = Substitute.For<IStrainerProcessor>();

        strainerProcessor
            .ApplyPagination(strainerModel, source)
            .Returns(Array.Empty<string>().AsQueryable());

        // Act
        var result = source.ApplyPagination(strainerModel, strainerProcessor);

        // Assert
        result.Should().BeEmpty();

        strainerProcessor.Received(1).ApplyPagination(strainerModel, source);
    }

    [Fact]
    public void ApplySorting_Should_Return_Source_WhenModelIsNull()
    {
        // Arrange
        var value = "foo";
        var source = new string[] { value }.AsQueryable();
        IStrainerModel strainerModel = null;
        var strainerProcessor = Substitute.For<IStrainerProcessor>();

        // Act
        source = source.ApplySorting(strainerModel, strainerProcessor);

        // Assert
        source.Should().BeEquivalentTo(value);

        strainerProcessor.DidNotReceive().ApplySorting(strainerModel, source);
    }

    [Fact]
    public void ApplySorting_Should_CallProcessor()
    {
        // Arrange
        var value = "foo";
        var source = new string[] { value }.AsQueryable();
        var strainerModel = Substitute.For<IStrainerModel>();
        var strainerProcessor = Substitute.For<IStrainerProcessor>();

        strainerProcessor
            .ApplySorting(strainerModel, source)
            .Returns(Array.Empty<string>().AsQueryable());

        // Act
        var result = source.ApplySorting(strainerModel, strainerProcessor);

        // Assert
        result.Should().BeEmpty();

        strainerProcessor.Received(1).ApplySorting(strainerModel, source);
    }
}
