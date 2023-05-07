using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Configuration;
using Fluorite.Strainer.Services.Metadata.FluentApi;
using Moq;

namespace Fluorite.Strainer.UnitTests.Services.Metadata.FluentApi
{
    public class FluentApiMetadataProviderTests
    {
        private readonly Mock<IStrainerOptionsProvider> _optionsProviderMock = new();
        private readonly Mock<IConfigurationMetadataProvider> _configurationMetadataProviderMock = new();

        [Fact]
        public void GetDefaultMetadata_ReturnsNull_When_NoMetadataAvailable()
        {
            // Arrange
            _optionsProviderMock
                .Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            _configurationMetadataProviderMock
                .Setup(x => x.GetDefaultMetadata())
                .Returns(new Dictionary<Type, IPropertyMetadata>());
            _configurationMetadataProviderMock
                .Setup(x => x.GetObjectMetadata())
                .Returns(new Dictionary<Type, IObjectMetadata>());
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider();

            // Act
            var metadata = fluentApiMetadataProvider.GetDefaultMetadata<Post>();

            // Assert
            metadata.Should().BeNull();
        }

        [Fact]
        public void GetDefaultMetadata_ReturnsMetadata_When_FromObject()
        {
            // Arrange
            _optionsProviderMock
                .Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            _configurationMetadataProviderMock
                .Setup(x => x.GetDefaultMetadata())
                .Returns(new Dictionary<Type, IPropertyMetadata>());
            var objectMetadata = new ObjectMetadata
            {
            };
            var objectMetadataDictionary = new Dictionary<Type, IObjectMetadata>
            {
                { typeof(Post), objectMetadata }
            };
            _configurationMetadataProviderMock
                .Setup(x => x.GetObjectMetadata())
                .Returns(objectMetadataDictionary);
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider();

            // Act
            var metadata = fluentApiMetadataProvider.GetDefaultMetadata<Post>();

            // Assert
            metadata.Should().NotBeNull();
            metadata.IsDefaultSorting.Should().BeTrue();
            metadata.IsDefaultSortingDescending.Should().Be(objectMetadata.IsDefaultSortingDescending);
            metadata.IsFilterable.Should().Be(objectMetadata.IsFilterable);
            metadata.IsSortable.Should().Be(objectMetadata.IsSortable);
            metadata.Name.Should().Be(objectMetadata.DefaultSortingPropertyName);
            metadata.PropertyInfo.Should().BeSameAs(objectMetadata.DefaultSortingPropertyInfo);
        }

        [Fact]
        public void GetDefaultMetadata_Returns_PropertyMetadata_FromDefaultMetadata()
        {
            // Arrange
            _optionsProviderMock
                .Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var propertyMetadata = new PropertyMetadata
            {
                IsDefaultSorting = true,
                IsFilterable = true,
                Name = nameof(Post.Id),
                PropertyInfo = typeof(Post).GetProperty(nameof(Post.Id)),
            };
            var defaultMetadataDictionary = new Dictionary<Type, IPropertyMetadata>
            {
                { typeof(Post), propertyMetadata }
            };
            _configurationMetadataProviderMock
                .Setup(x => x.GetDefaultMetadata())
                .Returns(defaultMetadataDictionary);
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider();

            // Act
            var metadata = fluentApiMetadataProvider.GetDefaultMetadata<Post>();

            // Assert
            metadata.Should().NotBeNull();
            metadata.Should().Be(propertyMetadata);
        }

        [Fact]
        public void GetPropertyMetadata_ReturnsNull_When_NoMetadataAvailable()
        {
            // Arrange
            _optionsProviderMock
                .Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var propertyMetadata = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>();
            _configurationMetadataProviderMock
                .Setup(x => x.GetPropertyMetadata())
                .Returns(propertyMetadata);
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider();

            // Act
            var metadata = fluentApiMetadataProvider.GetPropertyMetadata<Post>(
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
                .Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var propertyMetadata = new PropertyMetadata
            {
                Name = nameof(Post.Id),
            };
            var postMetadataDictionary = new Dictionary<string, IPropertyMetadata>
            {
                { nameof(Post.Id), propertyMetadata }
            };
            var propertyMetadataDictionary = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>
            {
                { typeof(Post), postMetadataDictionary },
            };
            _configurationMetadataProviderMock
                .Setup(x => x.GetPropertyMetadata())
                .Returns(propertyMetadataDictionary);
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider();

            // Act
            var metadata = fluentApiMetadataProvider.GetPropertyMetadata<Post>(
                isSortableRequired: false,
                isFilterableRequired: false,
                name: nameof(Post.Id));

            // Assert
            metadata.Should().NotBeNull();
            metadata.Should().BeSameAs(propertyMetadata);
        }

        [Fact]
        public void GetPropertyMetadata_Returns_PropertyMetadata_When_MarkedAsSortable()
        {
            // Arrange
            _optionsProviderMock
                .Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var propertyMetadata = new PropertyMetadata
            {
                Name = nameof(Post.Id),
                IsFilterable = true,
                IsSortable = true,
            };
            var postMetadataDictionary = new Dictionary<string, IPropertyMetadata>
            {
                { nameof(Post.Id), propertyMetadata }
            };
            var propertyMetadataDictionary = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>
            {
                { typeof(Post), postMetadataDictionary },
            };
            _configurationMetadataProviderMock
                .Setup(x => x.GetPropertyMetadata())
                .Returns(propertyMetadataDictionary);
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider();

            // Act
            var metadata = fluentApiMetadataProvider.GetPropertyMetadata<Post>(
                isSortableRequired: true,
                isFilterableRequired: true,
                name: nameof(Post.Id));

            // Assert
            metadata.Should().NotBeNull();
            metadata.Should().BeSameAs(propertyMetadata);
        }

        [Fact]
        public void GetPropertyMetadata_Returns_Null_With_FluentApiMetadataSourceType_Disabled()
        {
            // Arrange
            _optionsProviderMock
                .Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions { MetadataSourceType = MetadataSourceType.Attributes });
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider();

            // Act
            var metadatas = fluentApiMetadataProvider.GetAllPropertyMetadata();

            // Assert
            metadatas.Should().BeNull();
        }

        [Fact]
        public void GetPropertyMetadata_Returns_EmptyMetadata_When_NoMetadataIsAvailable()
        {
            // Arrange
            _optionsProviderMock
                .Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var propertyMetadataDictionary = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>();
            _configurationMetadataProviderMock
                .Setup(x => x.GetPropertyMetadata())
                .Returns(propertyMetadataDictionary);
            var objectMetadataDictionary = new Dictionary<Type, IObjectMetadata>();
            _configurationMetadataProviderMock
                .Setup(x => x.GetObjectMetadata())
                .Returns(objectMetadataDictionary);
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider();

            // Act
            var metadatas = fluentApiMetadataProvider.GetAllPropertyMetadata();

            // Assert
            metadatas.Should().BeEmpty();
        }

        private FluentApiMetadataProvider CreateFluentApiMetadataProvider()
        {
            return new FluentApiMetadataProvider(
                _optionsProviderMock.Object,
                _configurationMetadataProviderMock.Object);
        }

        private class Post
        {
            public int Id { get; set; }
        }

        private class Comment
        {
            public int Id { get; set; }
        }

        private class TestStrainerProcessor : StrainerProcessor
        {
            public TestStrainerProcessor(IStrainerContext context) : base(context)
            {

            }
        }
    }
}
