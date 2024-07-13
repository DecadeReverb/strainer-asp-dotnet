using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Extensions;

namespace Fluorite.Strainer.UnitTests.Services.Metadata;

public class MetadataFacadeTests
{
    [Fact]
    public void GetAllMetadata_Should_Return_EmptyDictionary_WhenNoMetadataIsReturnedFromProviders()
    {
        // Arrange
        var facade = new MetadataFacade(Enumerable.Empty<IMetadataProvider>());

        // Act
        var result = facade.GetAllMetadata();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Fact]
    public void GetAllMetadata_Should_Return_FirstMetadataReturnedFromProviders()
    {
        // Arrange
        var metadata =
            new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>
            {
                [typeof(Exception)] = new Dictionary<string, IPropertyMetadata>().ToReadOnly(),
            }.ToReadOnly();
        var providerMock = Substitute.For<IMetadataProvider>();
        providerMock.GetAllPropertyMetadata().Returns(metadata);
        var providers = new List<IMetadataProvider> { providerMock };
        var facade = new MetadataFacade(providers);

        // Act
        var result = facade.GetAllMetadata();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(metadata);

        providerMock.Received(1).GetAllPropertyMetadata();
    }

    [Fact]
    public void GetAllMetadata_Should_Return_FirstMetadataReturnedFromProviders_WhileIgnoringOtherProviders()
    {
        // Arrange
        var metadata =
            new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>
            {
                [typeof(Exception)] = new Dictionary<string, IPropertyMetadata>().ToReadOnly(),
            }.ToReadOnly();
        var provider1Mock = Substitute.For<IMetadataProvider>();
        provider1Mock.GetAllPropertyMetadata().Returns(metadata);
        var provider2Mock = Substitute.For<IMetadataProvider>();
        var providers = new List<IMetadataProvider> { provider1Mock, provider2Mock };
        var facade = new MetadataFacade(providers);

        // Act
        var result = facade.GetAllMetadata();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(metadata);

        provider1Mock.Received(1).GetAllPropertyMetadata();
        provider2Mock.DidNotReceive().GetAllPropertyMetadata();
    }

    [Fact]
    public void GetAllMetadata_Should_Return_FirstNonNullMetadataReturnedFromProviders()
    {
        // Arrange
        var metadata =
            new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>
            {
                [typeof(Exception)] = new Dictionary<string, IPropertyMetadata>().ToReadOnly(),
            }.ToReadOnly();
        var provider1Mock = Substitute.For<IMetadataProvider>();
        provider1Mock.GetAllPropertyMetadata().Returns((IReadOnlyDictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>)null);
        var provider2Mock = Substitute.For<IMetadataProvider>();
        provider2Mock.GetAllPropertyMetadata().Returns(metadata);
        var providers = new List<IMetadataProvider> { provider1Mock, provider2Mock };
        var facade = new MetadataFacade(providers);

        // Act
        var result = facade.GetAllMetadata();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(metadata);

        provider1Mock.Received(1).GetAllPropertyMetadata();
        provider2Mock.Received(1).GetAllPropertyMetadata();
    }

    [Fact]
    public void GetDefaultMetadata_Should_Return_Null_WhenThereIsNoProviders()
    {
        // Arrange
        var facade = new MetadataFacade(Enumerable.Empty<IMetadataProvider>());

        // Act
        var result = facade.GetDefaultMetadata<Exception>();

        // Assert
        result.Should().BeNull();
    }
}
