using FluentAssertions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Metadata;
using Moq;
using System;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Metadata
{
    public class PropertyMetadataMapperTests
    {
        [Fact]
        public void Mapper_Returns_Null_When_JustPropertyIsCalled()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);

            // Act
            mapper.Property<Post>(p => p.Id);
            var metadata = mapper.GetMetadata<Post>(
                isSortableRequired: false,
                isFilterableRequired: false,
                name: nameof(Post.Id));

            // Assert
            metadata.Should().BeNull();
        }

        [Fact]
        public void Mapper_Returns_Map_When_MarkedAsFilterable()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);
            mapper.Property<Post>(p => p.Id).IsFilterable();

            // Act
            var metadata = mapper.GetMetadata<Post>(
                isSortableRequired: false,
                isFilterableRequired: false,
                name: nameof(Post.Id));

            // Assert
            metadata.Name.Should().Be(nameof(Post.Id));
            metadata.PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }

        [Fact]
        public void Mapper_Returns_Map_When_MarkedAsSortable()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);
            mapper.Property<Post>(p => p.Id).IsSortable();

            // Act
            var metadata = mapper.GetMetadata<Post>(
                isSortableRequired: false,
                isFilterableRequired: false,
                name: nameof(Post.Id));

            // Assert
            metadata.Name.Should().Be(nameof(Post.Id));
            metadata.PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }

        [Fact]
        public void Mapper_Returns_DefaultMetadata_When_MarkedAsDefault()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);
            mapper.Property<Post>(p => p.Id).IsSortable().IsDefaultSort();

            // Act
            var metadata = mapper.GetDefaultMetadata<Post>();

            // Assert
            metadata.Name.Should().Be(nameof(Post.Id));
            metadata.IsDefaultSorting.Should().BeTrue();
            metadata.PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }

        [Fact]
        public void Mapper_Adds_Map_Via_AddMap()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);
            var metadata = new PropertyMetadata()
            {
                Name = nameof(Post.Id),
                PropertyInfo = typeof(Post).GetProperty(nameof(Post.Id)),
            };

            // Act
            mapper.AddPropertyMetadata<Post>(metadata);
            var result = mapper.GetMetadata<Post>(
                isSortableRequired: false,
                isFilterableRequired: false,
                name: nameof(Post.Id));

            // Assert
            result.Name.Should().Be(nameof(Post.Id));
            result.PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }

        [Fact]
        public void Mapper_Adds_Different_Metadata_For_Already_Existing_Property()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);
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
            var firstResult = mapper.GetMetadata<Post>(false, false, name: firstMetadata.Name);
            var secondResult = mapper.GetMetadata<Post>(false, false, name: secondMetadata.Name);

            // Assert
            firstResult.Should().Be(firstMetadata);
            secondResult.Should().Be(secondMetadata);
        }

        [Fact]
        public void Mapper_Adds_AlreadyExistingMaps_Via_AddMap_Without_Duplicating()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);
            var metadata = new PropertyMetadata()
            {
                Name = nameof(Post.Id),
                PropertyInfo = typeof(Post).GetProperty(nameof(Post.Id)),
            };

            // Act
            mapper.AddPropertyMetadata<Post>(metadata);
            mapper.AddPropertyMetadata<Post>(metadata);
            var result = mapper.GetMetadata<Post>(isSortableRequired: false, isFilterableRequired: false, nameof(Post.Id));

            // Assert
            result.Name.Should().Be(nameof(Post.Id));
            result.PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }

        [Fact]
        public void Mapper_Property_Throws_Exception_With_FluentApiMetadataSourceType_Disabled()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions { MetadataSourceType = MetadataSourceType.Attributes });
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => mapper.Property<Post>(p => p.Id));
        }

        [Fact]
        public void Mapper_AddMap_Throws_Exception_With_FluentApiMetadataSourceType_Disabled()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions { MetadataSourceType = MetadataSourceType.Attributes });
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);
            var metadata = new PropertyMetadata()
            {
                Name = nameof(Post.Id),
                PropertyInfo = typeof(Post).GetProperty(nameof(Post.Id)),
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => mapper.AddPropertyMetadata<Post>(metadata));
        }

        [Fact]
        public void Mapper_FindProperty_Returns_Null_With_FluentApiMetadataSourceType_Disabled()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions { MetadataSourceType = MetadataSourceType.Attributes });
            var optionsProvider = optionsMock.Object;
            var mapper = new MetadataMapper(optionsProvider);

            // Act
            var result = mapper.GetMetadata<Post>(
                isSortableRequired: true,
                isFilterableRequired: true,
                name: null);

            // Assert
            result.Should().BeNull();
        }

        private class Post
        {
            public int Id { get; set; }
        }
    }
}
