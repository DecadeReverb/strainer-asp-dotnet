using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Configuration;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Metadata.FluentApi;

namespace Fluorite.Strainer.UnitTests.Services.Metadata.FluentApi
{
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
            var metadata = _provider.GetDefaultMetadata<Post>();

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
                { typeof(Post), objectMetadata }
            };
            _configurationMetadataProviderMock
                .GetObjectMetadata()
                .Returns(objectMetadataDictionary);
            _propertyMetadataBuilderMock
                .BuildPropertyMetadata(objectMetadata)
                .Returns(propertyMetadata);

            // Act
            var metadata = _provider.GetDefaultMetadata<Post>();

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
                .GetDefaultMetadata()
                .Returns(defaultMetadataDictionary);

            // Act
            var metadata = _provider.GetDefaultMetadata<Post>();

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
            var metadata = _provider.GetPropertyMetadata<Post>(
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
                .GetPropertyMetadata()
                .Returns(propertyMetadataDictionary);

            // Act
            var metadata = _provider.GetPropertyMetadata<Post>(
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
                .GetStrainerOptions()
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
                .GetPropertyMetadata()
                .Returns(propertyMetadataDictionary);

            // Act
            var metadata = _provider.GetPropertyMetadata<Post>(
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

        private class Post
        {
            public int Id { get; set; }
        }

        private class Comment
        {
            public int Id { get; set; }
        }
    }
}
