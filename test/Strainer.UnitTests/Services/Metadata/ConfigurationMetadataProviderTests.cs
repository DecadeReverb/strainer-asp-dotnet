using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Configuration;
using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.UnitTests.Services.Metadata;

public class ConfigurationMetadataProviderTests
{
    private readonly IStrainerConfigurationProvider _strainerConfigurationProviderMock = Substitute.For<IStrainerConfigurationProvider>();

    private readonly ConfigurationMetadataProvider _provider;

    public ConfigurationMetadataProviderTests()
    {
        _provider = new ConfigurationMetadataProvider(_strainerConfigurationProviderMock);
    }

    [Fact]
    public void Should_Return_DefaultMetadataFromConfigurationProvider()
    {
        // Arrange
        var defaultMetadata = new Dictionary<Type, IPropertyMetadata>
        {
            [typeof(Blog)] = Substitute.For<IPropertyMetadata>(),
        };
        var strainerModuleMock = Substitute.For<IStrainerModule>();
        strainerModuleMock.DefaultMetadata.Returns(defaultMetadata);
        var configuration = new StrainerConfigurationBuilder()
            .WithDefaultMetadata([strainerModuleMock])
            .Build();
        _strainerConfigurationProviderMock
            .GetStrainerConfiguration()
            .Returns(configuration);

        // Act
        var result = _provider.GetDefaultMetadata();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo(defaultMetadata);

        _strainerConfigurationProviderMock
            .Received(1)
            .GetStrainerConfiguration();
    }

    [Fact]
    public void Should_Return_ObjectMetadataFromConfigurationProvider()
    {
        // Arrange
        var objectMetadata = new Dictionary<Type, IObjectMetadata>
        {
            [typeof(Blog)] = Substitute.For<IObjectMetadata>(),
        };
        var strainerModuleMock = Substitute.For<IStrainerModule>();
        strainerModuleMock.ObjectMetadata.Returns(objectMetadata);
        var configuration = new StrainerConfigurationBuilder()
            .WithObjectMetadata([strainerModuleMock])
            .Build();
        _strainerConfigurationProviderMock
            .GetStrainerConfiguration()
            .Returns(configuration);

        // Act
        var result = _provider.GetObjectMetadata();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo(objectMetadata);

        _strainerConfigurationProviderMock
            .Received(1)
            .GetStrainerConfiguration();
    }

    [Fact]
    public void Should_Return_PropertyMetadataFromConfigurationProvider()
    {
        // Arrange
        var propertyMetadata = new Dictionary<Type, IDictionary<string, IPropertyMetadata>>
        {
            [typeof(Blog)] = new Dictionary<string, IPropertyMetadata>
            {
                [nameof(Blog.Title)] = Substitute.For<IPropertyMetadata>(),
            },
        };
        var strainerModuleMock = Substitute.For<IStrainerModule>();
        strainerModuleMock.PropertyMetadata.Returns(propertyMetadata);
        var configuration = new StrainerConfigurationBuilder()
            .WithPropertyMetadata([strainerModuleMock])
            .Build();
        _strainerConfigurationProviderMock
            .GetStrainerConfiguration()
            .Returns(configuration);

        // Act
        var result = _provider.GetPropertyMetadata();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCount(1);
        result.Should().BeEquivalentTo(propertyMetadata);

        _strainerConfigurationProviderMock
            .Received(1)
            .GetStrainerConfiguration();
    }

    private class Blog
    {
        public string Title { get; set; }
    }
}
