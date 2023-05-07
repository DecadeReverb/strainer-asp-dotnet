using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Metadata.Attributes;
using Moq;
using System.Reflection;

namespace Fluorite.Strainer.UnitTests.Services.Metadata.Attributes
{
    public class AttributeMetadataProviderTests
    {
        private readonly Mock<IMetadataSourceTypeProvider> _metadataSourceTypeProviderMock = new();
        private readonly Mock<IMetadataAssemblySourceProvider> _metadataAssemblySourceProviderMock = new();
        private readonly Mock<IAttributeMetadataRetriever> _attributeMetadataRetrieverMock = new();
        private readonly Mock<IStrainerAttributeProvider> _strainerObjectAttributeProviderMock = new();
        private readonly AttributeMetadataProvider _provider;

        public AttributeMetadataProviderTests()
        {
            _provider = new AttributeMetadataProvider(
                _metadataSourceTypeProviderMock.Object,
                _metadataAssemblySourceProviderMock.Object,
                _attributeMetadataRetrieverMock.Object,
                _strainerObjectAttributeProviderMock.Object);
        }

        [Fact]
        public void Provider_Returns_AllMetadata()
        {
            // Arrange
            var assemblies = new[] { typeof(AttributeMetadataProviderTests).Assembly };
            var types = new[] { typeof(Post), typeof(Comment) };
            _metadataAssemblySourceProviderMock
                .Setup(x => x.GetAssemblies())
                .Returns(assemblies);
            _metadataSourceTypeProviderMock
                .Setup(x => x.GetSourceTypes(It.Is<Assembly[]>(x => x.SequenceEqual(assemblies))))
                .Returns(types);
            _attributeMetadataRetrieverMock
                .Setup(x => x.GetMetadataDictionaryFromObjectAttributes(types))
                .Returns(new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>
                {
                    {
                        typeof(Post),
                        new Dictionary<string, IPropertyMetadata>
                        {
                            { nameof(Post.Title), Mock.Of<IPropertyMetadata>() },
                        }
                    },
                });
            _attributeMetadataRetrieverMock
                .Setup(x => x.GetMetadataDictionaryFromPropertyAttributes(types))
                .Returns(new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>
                {
                    {
                        typeof(Comment),
                        new Dictionary<string, IPropertyMetadata>
                        {
                            { nameof(Comment.Id), Mock.Of<IPropertyMetadata>() },
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
        public void Provider_Returns_DefaultMetadata()
        {
            // Arrange
            _attributeMetadataRetrieverMock
                .Setup(x => x.GetDefaultMetadataFromObjectAttribute(typeof(Comment)))
                .Returns(Mock.Of<IPropertyMetadata>());

            // Act
            var result = _provider.GetDefaultMetadata<Comment>();

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void Provider_Returns_NoPropertyMetadata_WhenNoneAreMatchingConditions()
        {
            // Act
            var result = _provider.GetPropertyMetadata<Post>(
                isSortableRequired: true,
                isFilterableRequired: true,
                name: nameof(Post.Id));

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Provider_Returns_NoPropertyMetadata_WhenAttributeMetadataSource_Is_Disabled()
        {
            // Act
            var result = _provider.GetPropertyMetadata<Post>(
                isSortableRequired: true,
                isFilterableRequired: true,
                name: nameof(Post.Id));

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Provider_Returns_PropertyMetadata_FromStrainerAttribute()
        {
            // Arrange
            var propertyMetadata = new PropertyMetadata
            {
                Name = nameof(Post.Title),
                IsFilterable = true,
                IsSortable = true,
            };

            _attributeMetadataRetrieverMock
                .Setup(x => x.GetMetadataFromPropertyAttribute(typeof(Post), true, true, nameof(Post.Title)))
                .Returns(propertyMetadata);

            // Act
            var result = _provider.GetPropertyMetadata<Post>(
                isSortableRequired: true,
                isFilterableRequired: true,
                name: nameof(Post.Title));

            // Assert
            result.Should().NotBeNull();
            result.IsFilterable.Should().BeTrue();
            result.IsSortable.Should().BeTrue();
            result.Name.Should().Be(nameof(Post.Title));
        }

        [Fact]
        public void Provider_Returns_PropertyMetadata_FromStrainerObjectAttribute()
        {
            // Arrange
            var propertyMetadata = new PropertyMetadata
            {
                IsFilterable = true,
                IsSortable = true,
                Name = nameof(Comment.Id),
            };
            _attributeMetadataRetrieverMock
                .Setup(x => x.GetMetadataFromObjectAttribute(typeof(Comment), true, true, nameof(Comment.Id)))
                .Returns(propertyMetadata);

            // Act
            var result = _provider.GetPropertyMetadata<Comment>(
                isSortableRequired: true,
                isFilterableRequired: true,
                name: nameof(Comment.Id));

            // Assert
            result.Should().NotBeNull();
            result.IsFilterable.Should().BeTrue();
            result.IsSortable.Should().BeTrue();
            result.Name.Should().Be(nameof(Comment.Id));
        }

        [Fact]
        public void Provider_Returns_PropertyMetadatas_FromStrainerPropertyAttribute()
        {
            // Arrange
            var type = typeof(Post);
            var propertyMetadatas = new List<IPropertyMetadata>
            {
                Mock.Of<IPropertyMetadata>(),
            };
            _attributeMetadataRetrieverMock
                .Setup(x => x.GetMetadataFromPropertyAttribute(type))
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
                Mock.Of<IPropertyMetadata>(),
            };
            _attributeMetadataRetrieverMock
                .Setup(x => x.GetMetadataFromPropertyAttribute(type))
                .Returns<IEnumerable<IPropertyMetadata>>(null);
            _attributeMetadataRetrieverMock
                .Setup(x => x.GetMetadataFromObjectAttribute(type))
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
}
