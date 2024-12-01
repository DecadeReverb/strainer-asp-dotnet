using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Configuration;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Metadata.FluentApi;
using System.Reflection;

namespace Fluorite.Strainer.UnitTests.Services.Metadata.FluentApi;

public class FluentApiMetadataProviderTests
{
    private readonly IStrainerOptionsProvider _optionsProviderMock = Substitute.For<IStrainerOptionsProvider>();
    private readonly IConfigurationMetadataProvider _configurationMetadataProviderMock = Substitute.For<IConfigurationMetadataProvider>();
    private readonly IPropertyInfoProvider _propertyInfoProviderMock = Substitute.For<IPropertyInfoProvider>();
    private readonly IFluentApiPropertyMetadataBuilder _propertyMetadataBuilderMock = Substitute.For<IFluentApiPropertyMetadataBuilder>();

    private readonly FluentApiMetadataProvider _provider;

    public FluentApiMetadataProviderTests()
    {
        _provider = new FluentApiMetadataProvider(
            _optionsProviderMock,
            _configurationMetadataProviderMock,
            _propertyInfoProviderMock,
            _propertyMetadataBuilderMock);
    }

    [Fact]
    public void GetDefaultMetadata_ReturnsNull_When_NoMetadataAvailable()
    {
        // Arrange
        _optionsProviderMock
            .GetStrainerOptions()
            .Returns(new StrainerOptions());
        _configurationMetadataProviderMock
            .GetDefaultMetadata()
            .Returns(new Dictionary<Type, IPropertyMetadata>());
        _configurationMetadataProviderMock
            .GetObjectMetadata()
            .Returns(new Dictionary<Type, IObjectMetadata>());

        // Act
        var metadata = _provider.GetDefaultMetadata(typeof(Post));

        // Assert
        metadata.Should().BeNull();
    }

    [Fact]
    public void GetDefaultMetadata_ReturnsMetadata_When_FromObject()
    {
        // Arrange
        _optionsProviderMock
            .GetStrainerOptions()
            .Returns(new StrainerOptions());
        _configurationMetadataProviderMock
            .GetDefaultMetadata()
            .Returns(new Dictionary<Type, IPropertyMetadata>());
        var propertyMetadata = Substitute.For<IPropertyMetadata>();
        var objectMetadata = Substitute.For<IObjectMetadata>();
        var objectMetadataDictionary = new Dictionary<Type, IObjectMetadata>
        {
            { typeof(Post), objectMetadata },
        };
        _configurationMetadataProviderMock
            .GetObjectMetadata()
            .Returns(objectMetadataDictionary);
        _propertyMetadataBuilderMock
            .BuildPropertyMetadata(objectMetadata)
            .Returns(propertyMetadata);

        // Act
        var metadata = _provider.GetDefaultMetadata(typeof(Post));

        // Assert
        metadata.Should().NotBeNull();
        metadata.Should().BeSameAs(propertyMetadata);
    }

    [Fact]
    public void GetDefaultMetadata_Returns_PropertyMetadata_FromDefaultMetadata()
    {
        // Arrange
        _optionsProviderMock
            .GetStrainerOptions()
            .Returns(new StrainerOptions());
        var propertyMetadata = Substitute.For<IPropertyMetadata>();
        var defaultMetadataDictionary = new Dictionary<Type, IPropertyMetadata>
        {
            { typeof(Post), propertyMetadata },
        };
        _configurationMetadataProviderMock
            .GetDefaultMetadata()
            .Returns(defaultMetadataDictionary);

        // Act
        var metadata = _provider.GetDefaultMetadata(typeof(Post));

        // Assert
        metadata.Should().NotBeNull();
        metadata.Should().Be(propertyMetadata);
    }

    [Fact]
    public void GetPropertyMetadata_ReturnsNull_When_NoMetadataAvailable()
    {
        // Arrange
        _optionsProviderMock
            .GetStrainerOptions()
            .Returns(new StrainerOptions());
        var propertyMetadata = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>();
        _configurationMetadataProviderMock
            .GetPropertyMetadata()
            .Returns(propertyMetadata);

        // Act
        var metadata = _provider.GetPropertyMetadata(
            typeof(Post),
            isSortableRequired: false,
            isFilterableRequired: false,
            name: nameof(Post.Id));

        // Assert
        metadata.Should().BeNull();
    }

    [Fact]
    public void GetPropertyMetadata_Returns_PropertyMetadata()
    {
        // Arrange
        _optionsProviderMock
            .GetStrainerOptions()
            .Returns(new StrainerOptions());
        var propertyMetadata = Substitute.For<IPropertyMetadata>();
        var postMetadataDictionary = new Dictionary<string, IPropertyMetadata>
        {
            { nameof(Post.Id), propertyMetadata },
        };
        var propertyMetadataDictionary = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>
        {
            { typeof(Post), postMetadataDictionary },
        };
        _configurationMetadataProviderMock
            .GetPropertyMetadata()
            .Returns(propertyMetadataDictionary);

        // Act
        var metadata = _provider.GetPropertyMetadata(
            typeof(Post),
            isSortableRequired: false,
            isFilterableRequired: false,
            name: nameof(Post.Id));

        // Assert
        metadata.Should().NotBeNull();
        metadata.Should().BeSameAs(propertyMetadata);
    }

    [Theory]
    [InlineData(false, false, false, false)]
    [InlineData(false, false, true, false)]
    [InlineData(false, false, false, true)]
    [InlineData(false, false, true, true)]
    [InlineData(true, false, true, false)]
    [InlineData(true, false, true, true)]
    [InlineData(false, true, false, true)]
    [InlineData(false, true, true, true)]
    [InlineData(true, true, true, true)]
    public void GetPropertyMetadata_Returns_PropertyMetadata_WhenMatching(bool isFilterableRequired, bool isSortableRequired, bool isFilterable, bool isSortable)
    {
        // Arrange
        _optionsProviderMock
            .GetStrainerOptions()
            .Returns(new StrainerOptions());
        var propertyMetadata = Substitute.For<IPropertyMetadata>();
        propertyMetadata.IsFilterable.Returns(isFilterable);
        propertyMetadata.IsSortable.Returns(isSortable);
        var postMetadataDictionary = new Dictionary<string, IPropertyMetadata>
        {
            { nameof(Post.Id), propertyMetadata },
        };
        var propertyMetadataDictionary = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>
        {
            { typeof(Post), postMetadataDictionary },
        };
        _configurationMetadataProviderMock
            .GetPropertyMetadata()
            .Returns(propertyMetadataDictionary);

        // Act
        var metadata = _provider.GetPropertyMetadata(
            typeof(Post),
            isSortableRequired,
            isFilterableRequired,
            name: nameof(Post.Id));

        // Assert
        metadata.Should().NotBeNull();
        metadata.Should().BeSameAs(propertyMetadata);
    }

    [Theory]
    [InlineData(false, true, false, false)]
    [InlineData(false, true, true, false)]
    [InlineData(true, false, false, false)]
    [InlineData(true, false, false, true)]
    [InlineData(true, true, false, false)]
    [InlineData(true, true, true, false)]
    [InlineData(true, true, false, true)]
    public void GetPropertyMetadata_Returns_Null_WhenNotMatching(bool isFilterableRequired, bool isSortableRequired, bool isFilterable, bool isSortable)
    {
        // Arrange
        _optionsProviderMock
            .GetStrainerOptions()
            .Returns(new StrainerOptions());
        var propertyMetadata = Substitute.For<IPropertyMetadata>();
        propertyMetadata.IsFilterable.Returns(isFilterable);
        propertyMetadata.IsSortable.Returns(isSortable);
        var postMetadataDictionary = new Dictionary<string, IPropertyMetadata>
        {
            { nameof(Post.Id), propertyMetadata },
        };
        var propertyMetadataDictionary = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>
        {
            { typeof(Post), postMetadataDictionary },
        };
        _configurationMetadataProviderMock
            .GetPropertyMetadata()
            .Returns(propertyMetadataDictionary);

        // Act
        var metadata = _provider.GetPropertyMetadata(
            typeof(Post),
            isSortableRequired,
            isFilterableRequired,
            name: nameof(Post.Id));

        // Assert
        metadata.Should().BeNull();
    }

    [Theory]
    [InlineData(false, false, false, false)]
    [InlineData(false, false, false, true)]
    [InlineData(false, false, true, false)]
    [InlineData(false, false, true, true)]
    [InlineData(false, true, false, true)]
    [InlineData(false, true, true, true)]
    [InlineData(true, false, true, false)]
    [InlineData(true, false, true, true)]
    [InlineData(true, true, true, true)]
    public void GetPropertyMetadata_Returns_ObjectMetadataAsFallback(bool isSortableRequired, bool isFilterableRequired, bool isSortable, bool isFilterable)
    {
        // Arrange
        var name = nameof(Post.Id);
        _optionsProviderMock
            .GetStrainerOptions()
            .Returns(new StrainerOptions());
        var objectMetadata = Substitute.For<IObjectMetadata>();
        objectMetadata.IsFilterable.Returns(isFilterable);
        objectMetadata.IsSortable.Returns(isSortable);
        var propertyMetadata = Substitute.For<IPropertyMetadata>();
        var propertyInfo = Substitute.For<PropertyInfo>();
        var propertyMetadataDictionary = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>();
        var objectMetadataDictionary = new Dictionary<Type, IObjectMetadata>
        {
            { typeof(Post), objectMetadata },
        };
        _configurationMetadataProviderMock
            .GetPropertyMetadata()
            .Returns(propertyMetadataDictionary);
        _configurationMetadataProviderMock
            .GetObjectMetadata()
            .Returns(objectMetadataDictionary);
        _propertyInfoProviderMock
            .GetPropertyInfo(typeof(Post), name)
            .Returns(propertyInfo);
        _propertyMetadataBuilderMock
            .BuildPropertyMetadataFromPropertyInfo(objectMetadata, propertyInfo)
            .Returns(propertyMetadata);

        // Act
        var metadata = _provider.GetPropertyMetadata(
            typeof(Post),
            isSortableRequired,
            isFilterableRequired,
            name);

        // Assert
        metadata.Should().NotBeNull();
        metadata.Should().BeSameAs(propertyMetadata);
    }

    [Theory]
    [InlineData(true, false, false, false)]
    [InlineData(true, false, false, true)]
    [InlineData(false, true, false, false)]
    [InlineData(false, true, true, false)]
    [InlineData(true, true, false, false)]
    [InlineData(true, true, true, false)]
    [InlineData(true, true, false, true)]
    public void GetPropertyMetadata_Returns_NullObjectMetadata_WhenNotMatching(bool isSortableRequired, bool isFilterableRequired, bool isSortable, bool isFilterable)
    {
        // Arrange
        var name = nameof(Post.Id);
        _optionsProviderMock
            .GetStrainerOptions()
            .Returns(new StrainerOptions());
        var objectMetadata = Substitute.For<IObjectMetadata>();
        objectMetadata.IsFilterable.Returns(isFilterable);
        objectMetadata.IsSortable.Returns(isSortable);
        var propertyMetadata = Substitute.For<IPropertyMetadata>();
        var propertyInfo = Substitute.For<PropertyInfo>();
        var propertyMetadataDictionary = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>();
        var objectMetadataDictionary = new Dictionary<Type, IObjectMetadata>
        {
            { typeof(Post), objectMetadata },
        };
        _configurationMetadataProviderMock
            .GetPropertyMetadata()
            .Returns(propertyMetadataDictionary);
        _configurationMetadataProviderMock
            .GetObjectMetadata()
            .Returns(objectMetadataDictionary);
        _propertyInfoProviderMock
            .GetPropertyInfo(typeof(Post), name)
            .Returns(propertyInfo);
        _propertyMetadataBuilderMock
            .BuildPropertyMetadataFromPropertyInfo(objectMetadata, propertyInfo)
            .Returns(propertyMetadata);

        // Act
        var metadata = _provider.GetPropertyMetadata(
            typeof(Post),
            isSortableRequired,
            isFilterableRequired,
            name);

        // Assert
        metadata.Should().BeNull();
    }

    [Theory]
    [InlineData(false, true, false, false)]
    [InlineData(false, true, true, false)]
    [InlineData(true, false, false, true)]
    [InlineData(true, false, false, false)]
    [InlineData(true, true, false, false)]
    public void GetPropertyMetadata_ReturnsNull_WhenObjectMetadataIsNotMatchingCriteria(bool isSortableRequired, bool isFilterableRequired, bool isSortable, bool isFilterable)
    {
        // Arrange
        var name = nameof(Post.Id);
        _optionsProviderMock
            .GetStrainerOptions()
            .Returns(new StrainerOptions());
        var objectMetadata = Substitute.For<IObjectMetadata>();
        objectMetadata.IsFilterable.Returns(isFilterable);
        objectMetadata.IsSortable.Returns(isSortable);
        var propertyMetadataDictionary = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>();
        var objectMetadataDictionary = new Dictionary<Type, IObjectMetadata>
        {
            { typeof(Post), objectMetadata },
        };
        _configurationMetadataProviderMock
            .GetPropertyMetadata()
            .Returns(propertyMetadataDictionary);
        _configurationMetadataProviderMock
            .GetObjectMetadata()
            .Returns(objectMetadataDictionary);

        // Act
        var metadata = _provider.GetPropertyMetadata(
            typeof(Post),
            isSortableRequired,
            isFilterableRequired,
            name);

        // Assert
        metadata.Should().BeNull();
    }

    [Fact]
    public void GetPropertyMetadata_Returns_Null_With_FluentApiMetadataSourceType_Disabled()
    {
        // Arrange
        _optionsProviderMock
            .GetStrainerOptions()
            .Returns(new StrainerOptions { MetadataSourceType = MetadataSourceType.Attributes });

        // Act
        var metadatas = _provider.GetAllPropertyMetadata();

        // Assert
        metadatas.Should().BeNull();
    }

    [Fact]
    public void GetPropertyMetadata_Returns_EmptyMetadata_When_NoMetadataIsAvailable()
    {
        // Arrange
        _optionsProviderMock
            .GetStrainerOptions()
            .Returns(new StrainerOptions());
        var propertyMetadataDictionary = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>();
        _configurationMetadataProviderMock
            .GetPropertyMetadata()
            .Returns(propertyMetadataDictionary);
        var objectMetadataDictionary = new Dictionary<Type, IObjectMetadata>();
        _configurationMetadataProviderMock
            .GetObjectMetadata()
            .Returns(objectMetadataDictionary);

        // Act
        var metadatas = _provider.GetAllPropertyMetadata();

        // Assert
        metadatas.Should().BeEmpty();
    }

    [Fact]
    public void GetPropertyMetadatas_Returns_NullWhenNoMetadataIsFound()
    {
        // Arrange
        _optionsProviderMock
            .GetStrainerOptions()
            .Returns(new StrainerOptions());

        // Act
        var metadatas = _provider.GetPropertyMetadatas(typeof(Post));

        // Assert
        metadatas.Should().BeNull();
    }

    [Fact]
    public void GetPropertyMetadatas_Returns_PropertyMetadatas()
    {
        // Arrange
        _optionsProviderMock
            .GetStrainerOptions()
            .Returns(new StrainerOptions());
        var propertyMetadataDictionary = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>
        {
            { typeof(Post), new Dictionary<string, IPropertyMetadata>() },
        };
        _configurationMetadataProviderMock
            .GetPropertyMetadata()
            .Returns(propertyMetadataDictionary);

        // Act
        var metadatas = _provider.GetPropertyMetadatas(typeof(Post));

        // Assert
        metadatas.Should().NotBeNull();
        metadatas.Should().BeEmpty();
    }

    [Fact]
    public void GetPropertyMetadatas_Returns_PropertyMetadatasFromObject()
    {
        // Arrange
        _optionsProviderMock
            .GetStrainerOptions()
            .Returns(new StrainerOptions());
        var objectMetadata = Substitute.For<IObjectMetadata>();
        var propertyMetadata = Substitute.For<IPropertyMetadata>();
        var propertyInfo = Substitute.For<PropertyInfo>();
        var propertyInfos = new[] { propertyInfo };
        var propertyMetadataDictionary = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>();
        var objectMetadataDictionary = new Dictionary<Type, IObjectMetadata>
        {
            { typeof(Post), objectMetadata },
        };
        _configurationMetadataProviderMock
            .GetPropertyMetadata()
            .Returns(propertyMetadataDictionary);
        _configurationMetadataProviderMock
            .GetObjectMetadata()
            .Returns(objectMetadataDictionary);
        _propertyInfoProviderMock
            .GetPropertyInfos(typeof(Post))
            .Returns(propertyInfos);
        _propertyMetadataBuilderMock
            .BuildPropertyMetadataFromPropertyInfo(objectMetadata, propertyInfo)
            .Returns(propertyMetadata);

        // Act
        var metadatas = _provider.GetPropertyMetadatas(typeof(Post));

        // Assert
        metadatas.Should().NotBeNullOrEmpty();
        metadatas.Should().BeEquivalentTo([propertyMetadata]);
    }

    private class Post
    {
        public int Id { get; set; }
    }
}
