using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Metadata.Attributes;
using NSubstitute.ReturnsExtensions;

namespace Fluorite.Strainer.UnitTests.Services.Metadata.Attributes;

public class AttributeMetadataProviderTests
{
    private readonly IMetadataSourceTypeProvider _metadataSourceTypeProviderMock = Substitute.For<IMetadataSourceTypeProvider>();
    private readonly IMetadataAssemblySourceProvider _metadataAssemblySourceProviderMock = Substitute.For<IMetadataAssemblySourceProvider>();
    private readonly IAttributeMetadataRetriever _attributeMetadataRetrieverMock = Substitute.For<IAttributeMetadataRetriever>();

    private readonly AttributeMetadataProvider _provider;

    public AttributeMetadataProviderTests()
    {
        _provider = new AttributeMetadataProvider(
            _metadataSourceTypeProviderMock,
            _metadataAssemblySourceProviderMock,
            _attributeMetadataRetrieverMock);
    }

    [Fact]
    public void Provider_Returns_AllMetadata()
    {
        // Arrange
        var assemblies = new[] { typeof(AttributeMetadataProviderTests).Assembly };
        var types = new[] { typeof(Post), typeof(Comment) };
        _metadataAssemblySourceProviderMock
            .GetAssemblies()
            .Returns(assemblies);
        _metadataSourceTypeProviderMock
            .GetSourceTypes(assemblies)
            .Returns(types);
        _attributeMetadataRetrieverMock
            .GetMetadataDictionaryFromObjectAttributes(types)
            .Returns(new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>
            {
                {
                    typeof(Post),
                    new Dictionary<string, IPropertyMetadata>
                    {
                        { nameof(Post.Title), Substitute.For<IPropertyMetadata>() },
                    }
                },
            });
        _attributeMetadataRetrieverMock
            .GetMetadataDictionaryFromPropertyAttributes(types)
            .Returns(new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>
            {
                {
                    typeof(Comment),
                    new Dictionary<string, IPropertyMetadata>
                    {
                        { nameof(Comment.Id), Substitute.For<IPropertyMetadata>() },
                    }
                },
            });

        // Act
        var result = _provider.GetAllPropertyMetadata();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().HaveCount(2);
        result.Keys.Should().BeEquivalentTo(types);
        result.Values.Should().OnlyContain(x => x.Any());
    }

    [Fact]
    public void Provider_Returns_DefaultMetadata_ForObject()
    {
        // Arrange
        _attributeMetadataRetrieverMock
            .GetDefaultMetadataFromObjectAttribute(typeof(Comment))
            .Returns(Substitute.For<IPropertyMetadata>());

        // Act
        var result = _provider.GetDefaultMetadata(typeof(Comment));

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void Provider_Returns_DefaultMetadata_ForProperty()
    {
        // Arrange
        _attributeMetadataRetrieverMock
            .GetDefaultMetadataFromPropertyAttribute(typeof(Comment))
            .Returns(Substitute.For<IPropertyMetadata>());

        // Act
        var result = _provider.GetDefaultMetadata(typeof(Comment));

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void Provider_Returns_NoPropertyMetadata_WhenProviderReturnsNull()
    {
        // Arrange
        var name = nameof(Post.Id);

        _attributeMetadataRetrieverMock
            .GetMetadataFromPropertyAttribute(typeof(Post), Arg.Any<bool>(), Arg.Any<bool>(), name)
            .ReturnsNull();
        _attributeMetadataRetrieverMock
            .GetMetadataFromObjectAttribute(typeof(Post), Arg.Any<bool>(), Arg.Any<bool>(), name)
            .ReturnsNull();

        // Act
        var result = _provider.GetPropertyMetadata(
            typeof(Post),
            isSortableRequired: true,
            isFilterableRequired: true,
            name);

        // Assert
        result.Should().BeNull();
    }

    [Fact]
    public void Provider_Returns_PropertyMetadata_FromStrainerAttribute()
    {
        // Arrange
        var propertyMetadata = Substitute.For<IPropertyMetadata>();

        _attributeMetadataRetrieverMock
            .GetMetadataFromPropertyAttribute(typeof(Post), true, true, nameof(Post.Title))
            .Returns(propertyMetadata);

        // Act
        var result = _provider.GetPropertyMetadata(
            typeof(Post),
            isSortableRequired: true,
            isFilterableRequired: true,
            name: nameof(Post.Title));

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(propertyMetadata);
    }

    [Fact]
    public void Provider_Returns_PropertyMetadata_FromStrainerObjectAttribute()
    {
        // Arrange
        var propertyMetadata = Substitute.For<IPropertyMetadata>();

        _attributeMetadataRetrieverMock
            .GetMetadataFromPropertyAttribute(typeof(Comment), true, true, nameof(Comment.Id))
            .ReturnsNull();
        _attributeMetadataRetrieverMock
            .GetMetadataFromObjectAttribute(typeof(Comment), true, true, nameof(Comment.Id))
            .Returns(propertyMetadata);

        // Act
        var result = _provider.GetPropertyMetadata(
            typeof(Comment),
            isSortableRequired: true,
            isFilterableRequired: true,
            name: nameof(Comment.Id));

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(propertyMetadata);
    }

    [Fact]
    public void Provider_Returns_PropertyMetadatas_FromStrainerPropertyAttribute()
    {
        // Arrange
        var type = typeof(Post);
        var propertyMetadatas = new List<IPropertyMetadata>
        {
            Substitute.For<IPropertyMetadata>(),
        };
        _attributeMetadataRetrieverMock
            .GetMetadataFromPropertyAttribute(type)
            .Returns(propertyMetadatas);

        // Act
        var result = _provider.GetPropertyMetadatas(type);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(propertyMetadatas);
    }

    [Fact]
    public void Provider_Returns_PropertyMetadatas_FromStrainerObjectAttribute()
    {
        // Arrange
        var type = typeof(Post);
        var propertyMetadatas = new List<IPropertyMetadata>
        {
            Substitute.For<IPropertyMetadata>(),
        };
        _attributeMetadataRetrieverMock
            .GetMetadataFromPropertyAttribute(type)
            .ReturnsNull();
        _attributeMetadataRetrieverMock
            .GetMetadataFromObjectAttribute(type)
            .Returns(propertyMetadatas);

        // Act
        var result = _provider.GetPropertyMetadatas(type);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(propertyMetadatas);
    }

    private class Post
    {
        public int Id { get; set; }

        [StrainerProperty]
        public string Title { get; set; }
    }

    [StrainerObject(nameof(Id))]
    private class Comment
    {
        public int Id { get; set; }
    }
}
