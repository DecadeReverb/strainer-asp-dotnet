using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Metadata;

namespace Fluorite.Strainer.UnitTests.Services.Metadata
{
    public class MetadataMapperTests
    {
        [Fact]
        public void AddPropertyMetadata_Adds_PropertyMetadata_Via_AddPropertyMetadata()
        {
            // Arrange
            var optionsMock = Substitute.For<IStrainerOptionsProvider>();
            optionsMock
                .GetStrainerOptions()
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock;
            var propertyInfoProviderMock = Substitute.For<IPropertyInfoProvider>();
            var mapper = new MetadataMapper(optionsProvider, propertyInfoProviderMock);
            var metadata = new PropertyMetadata()
            {
                Name = nameof(Post.Id),
                PropertyInfo = typeof(Post).GetProperty(nameof(Post.Id)),
            };

            // Act
            mapper.AddPropertyMetadata<Post>(metadata);

            // Assert
            mapper.PropertyMetadata.Should().HaveCount(1);
            mapper.PropertyMetadata.First().Value.Should().HaveCount(1);
            mapper.PropertyMetadata.First().Value.First().Value.Name.Should().Be(nameof(Post.Id));
            mapper.PropertyMetadata.First().Value.First().Value.PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }

        [Fact]
        public void AddPropertyMetadata_Adds_Different_PropertyMetadata_For_Already_Existing_PropertyMetadata()
        {
            // Arrange
            var optionsMock = Substitute.For<IStrainerOptionsProvider>();
            optionsMock
                .GetStrainerOptions()
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock;
            var propertyInfoProviderMock = Substitute.For<IPropertyInfoProvider>();
            var mapper = new MetadataMapper(optionsProvider, propertyInfoProviderMock);
            var firstMetadata = new PropertyMetadata()
            {
                Name = "first",
                PropertyInfo = typeof(Post).GetProperty(nameof(Post.Id)),
            };
            var secondMetadata = new PropertyMetadata()
            {
                IsFilterable = true,
                Name = "second",
                PropertyInfo = typeof(Post).GetProperty(nameof(Post.Id)),
            };

            // Act
            mapper.AddPropertyMetadata<Post>(firstMetadata);
            mapper.AddPropertyMetadata<Post>(secondMetadata);

            // Assert
            mapper.PropertyMetadata.Should().HaveCount(1);
            mapper.PropertyMetadata.First().Value.Should().HaveCount(2);
            mapper.PropertyMetadata.First().Value.First().Value.Name.Should().Be(firstMetadata.Name);
            mapper.PropertyMetadata.First().Value.First().Value.PropertyInfo.Should().BeSameAs(firstMetadata.PropertyInfo);
            mapper.PropertyMetadata.First().Value.Last().Value.Name.Should().Be(secondMetadata.Name);
            mapper.PropertyMetadata.First().Value.Last().Value.PropertyInfo.Should().BeSameAs(secondMetadata.PropertyInfo);
        }

        [Fact]
        public void AddPropertyMetadata_Adds_AlreadyExistingPropertyMetadata_Via_AddPropertyMetadata_Without_Duplicating()
        {
            // Arrange
            var optionsMock = Substitute.For<IStrainerOptionsProvider>();
            optionsMock
                .GetStrainerOptions()
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock;
            var propertyInfoProviderMock = Substitute.For<IPropertyInfoProvider>();
            var mapper = new MetadataMapper(optionsProvider, propertyInfoProviderMock);
            var metadata = new PropertyMetadata()
            {
                Name = nameof(Post.Id),
                PropertyInfo = typeof(Post).GetProperty(nameof(Post.Id)),
            };

            // Act
            mapper.AddPropertyMetadata<Post>(metadata);
            mapper.AddPropertyMetadata<Post>(metadata);

            // Assert
            mapper.PropertyMetadata.Should().HaveCount(1);
            mapper.PropertyMetadata.First().Value.Should().HaveCount(1);
            mapper.PropertyMetadata.First().Value.First().Value.Name.Should().Be(nameof(Post.Id));
            mapper.PropertyMetadata.First().Value.First().Value.PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }

        [Fact]
        public void Property_Throws_Exception_With_FluentApiMetadataSourceType_Disabled()
        {
            // Arrange
            var optionsMock = Substitute.For<IStrainerOptionsProvider>();
            optionsMock
                .GetStrainerOptions()
                .Returns(new StrainerOptions { MetadataSourceType = MetadataSourceType.Attributes });
            var optionsProvider = optionsMock;
            var propertyInfoProviderMock = Substitute.For<IPropertyInfoProvider>();
            var mapper = new MetadataMapper(optionsProvider, propertyInfoProviderMock);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => mapper.Property<Post>(p => p.Id));
        }

        [Fact]
        public void AddPropertyMetadata_Throws_Exception_With_FluentApiMetadataSourceType_Disabled()
        {
            // Arrange
            var optionsMock = Substitute.For<IStrainerOptionsProvider>();
            optionsMock
                .GetStrainerOptions()
                .Returns(new StrainerOptions { MetadataSourceType = MetadataSourceType.Attributes });
            var optionsProvider = optionsMock;
            var propertyInfoProviderMock = Substitute.For<IPropertyInfoProvider>();
            var mapper = new MetadataMapper(optionsProvider, propertyInfoProviderMock);
            var metadata = new PropertyMetadata()
            {
                Name = nameof(Post.Id),
                PropertyInfo = typeof(Post).GetProperty(nameof(Post.Id)),
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => mapper.AddPropertyMetadata<Post>(metadata));
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
