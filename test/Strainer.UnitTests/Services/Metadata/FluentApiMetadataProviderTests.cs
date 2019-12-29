using FluentAssertions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Metadata;
using Moq;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Metadata
{
    public class FluentApiMetadataProviderTests
    {
        [Fact]
        public void GetDefaultMetadata_ReturnsMetadata_When_JustObjectIsCalled()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);

            // Act
            mapper.Object<Post>(p => p.Id);
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider(optionsProvider, mapper);
            var metadata = fluentApiMetadataProvider.GetDefaultMetadata<Post>();

            // Assert
            metadata.Should().NotBeNull();
            metadata.IsFilterable.Should().BeFalse();
            metadata.IsSortable.Should().BeFalse();
            metadata.IsDefaultSorting.Should().BeTrue();
        }

        [Fact]
        public void GetDefaultMetadata_Returns_PropertyMetadata_Added_Via_ObjectBuilder_When_MarkedAsFilterable()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);

            // Act
            mapper.Object<Post>(p => p.Id).IsFilterable();
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider(optionsProvider, mapper);
            var metadata = fluentApiMetadataProvider.GetDefaultMetadata<Post>();

            // Assert
            metadata.Should().NotBeNull();
            metadata.Name.Should().Be(nameof(Post.Id));
            metadata.IsFilterable.Should().BeTrue();
            metadata.PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }

        [Fact]
        public void GetDefaultMetadata_Returns_ObjectMetadata_Added_Via_ObjectBuilder_When_MarkedAsSortable()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);

            // Act
            mapper.Object<Post>(p => p.Id).IsSortable();
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider(optionsProvider, mapper);
            var metadata = fluentApiMetadataProvider.GetDefaultMetadata<Post>();

            // Assert
            metadata.Should().NotBeNull();
            metadata.Name.Should().Be(nameof(Post.Id));
            metadata.IsSortable.Should().BeTrue();
            metadata.PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }

        [Fact]
        public void GetPropertyMetadata_ReturnsMetdata_When_JustPropertyIsCalled()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);

            // Act
            mapper.Property<Post>(p => p.Id);
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider(optionsProvider, mapper);
            var metadata = fluentApiMetadataProvider.GetPropertyMetadata<Post>(
                isSortableRequired: false,
                isFilterableRequired: false,
                name: nameof(Post.Id));

            // Assert
            metadata.Should().NotBeNull();
            metadata.IsFilterable.Should().BeFalse();
            metadata.IsSortable.Should().BeFalse();
            metadata.IsDefaultSorting.Should().BeFalse();
        }

        [Fact]
        public void GetPropertyMetadata_ReturnsNull_When_JustObjectIsCalled()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);

            // Act
            mapper.Object<Post>(p => p.Id);
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider(optionsProvider, mapper);
            var metadata = fluentApiMetadataProvider.GetPropertyMetadata<Post>(
                isSortableRequired: false,
                isFilterableRequired: false,
                name: nameof(Post.Id));

            // Assert
            metadata.Should().BeNull();
        }

        [Fact]
        public void GetPropertyMetadata_Returns_PropertyMetadata_Added_Via_PropertyBuilder_When_MarkedAsFilterable()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);

            // Act
            mapper.Property<Post>(p => p.Id).IsFilterable();
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider(optionsProvider, mapper);
            var metadata = fluentApiMetadataProvider.GetPropertyMetadata<Post>(
                isSortableRequired: false,
                isFilterableRequired: true,
                name: nameof(Post.Id));

            // Assert
            metadata.Should().NotBeNull();
            metadata.Name.Should().Be(nameof(Post.Id));
            metadata.IsFilterable.Should().BeTrue();
            metadata.PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }

        [Fact]
        public void GetPropertyMetadata_Returns_PropertyMetadata_Added_Via_PropertyBuilder_When_MarkedAsSortable()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);

            // Act
            mapper.Property<Post>(p => p.Id).IsSortable();
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider(optionsProvider, mapper);
            var metadata = fluentApiMetadataProvider.GetPropertyMetadata<Post>(
                isSortableRequired: true,
                isFilterableRequired: false,
                name: nameof(Post.Id));

            // Assert
            metadata.Should().NotBeNull();
            metadata.Name.Should().Be(nameof(Post.Id));
            metadata.IsSortable.Should().BeTrue();
            metadata.PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }

        [Fact]
        public void GetPropertyMetadata_Returns_Null_Added_Via_ObjectBuilder_When_MarkedAsFilterable()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);

            // Act
            mapper.Object<Post>(p => p.Id).IsFilterable();
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider(optionsProvider, mapper);
            var metadata = fluentApiMetadataProvider.GetPropertyMetadata<Post>(
                isSortableRequired: false,
                isFilterableRequired: true,
                name: nameof(Post.Id));

            // Assert
            metadata.Should().BeNull();
        }

        [Fact]
        public void GetPropertyMetadata_Returns_Null_Added_Via_ObjectBuilder_When_MarkedAsSortable()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);

            // Act
            mapper.Object<Post>(p => p.Id).IsSortable();
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider(optionsProvider, mapper);
            var metadata = fluentApiMetadataProvider.GetPropertyMetadata<Post>(
                isSortableRequired: true,
                isFilterableRequired: false,
                name: nameof(Post.Id));

            // Assert
            metadata.Should().BeNull();
        }

        [Fact]
        public void GetPropertyMetadata_Returns_DefaultPropertyMetadata_Added_Via_ObjectBuilder()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);

            // Act
            mapper.Object<Post>(p => p.Id);
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider(optionsProvider, mapper);
            var metadata = fluentApiMetadataProvider.GetDefaultMetadata<Post>();

            // Assert
            metadata.Should().NotBeNull();
            metadata.Name.Should().Be(nameof(Post.Id));
            metadata.IsSortable.Should().BeFalse();
            metadata.IsDefaultSorting.Should().BeTrue();
            metadata.PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }

        // TODO:
        // Tests below should be working after implementing modules configuration.

        //[Fact]
        //public void MetadataMapper_Works_For_Object_Attribute()
        //{
        //    // Arrange
        //    var services = new ServiceCollection();
        //    services.AddStrainer<TestStrainerProcessor>();
        //    services.AddScoped<MetadataMapper>();
        //    var serviceProvider = services.BuildServiceProvider();

        //    // Act
        //    var attributeMetadataProvider = serviceProvider.GetRequiredService<MetadataMapper>();
        //    var metadatas = attributeMetadataProvider.GetPropertyMetadatas<Post>();

        //    // Assert
        //    metadatas.Should().NotBeNullOrEmpty();
        //    metadatas.Should().HaveSameCount(typeof(Post).GetProperties());
        //    metadatas.First().Name.Should().Be(nameof(Post.Id));
        //}

        //[Fact]
        //public void MetadataMapper_Works_For_Property_Attribute()
        //{
        //    // Arrange
        //    var services = new ServiceCollection();
        //    services.AddStrainer<TestStrainerProcessor>();
        //    services.AddScoped<MetadataMapper>();
        //    var serviceProvider = services.BuildServiceProvider();

        //    // Act
        //    var attributeMetadataProvider = serviceProvider.GetRequiredService<MetadataMapper>();
        //    var metadatas = attributeMetadataProvider.GetPropertyMetadatas<Comment>();

        //    // Assert
        //    metadatas.Should().NotBeNullOrEmpty();
        //    metadatas.Should().HaveSameCount(typeof(Post).GetProperties());
        //}

        [Fact]
        public void GetPropertyMetadata_Returns_Null_With_FluentApiMetadataSourceType_Disabled()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions { MetadataSourceType = MetadataSourceType.Attributes });
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);
            var fluentApiMetadataProvider = CreateFluentApiMetadataProvider(optionsProvider, mapper);

            // Act
            var metadatas = fluentApiMetadataProvider.GetAllPropertyMetadata();

            // Assert
            metadatas.Should().BeNull();
        }

        private FluentApiMetadataProvider CreateFluentApiMetadataProvider(
            IStrainerOptionsProvider optionsProvider,
            MetadataMapper mapper)
        {
            var defaultMetadata = new DefaultMetadataDictionary(mapper.DefaultMetadata);
            var objectMetadata = new ObjectMetadataDictionary(mapper.ObjectMetadata);
            var propertyMetadata = new PropertyMetadataDictionary(mapper.PropertyMetadata);
            var fluentApiMetadataProvider = new FluentApiMetadataProvider(
                optionsProvider,
                defaultMetadata,
                objectMetadata,
                propertyMetadata);

            return fluentApiMetadataProvider;
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
