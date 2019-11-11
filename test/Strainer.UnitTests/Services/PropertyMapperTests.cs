using FluentAssertions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Moq;
using System;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services
{
    public class PropertyMapperTests
    {
        [Fact]
        public void Mapper_Returns_Null_When_JustPropertyIsCalled()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new PropertyMapper(optionsProvider);

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
            var mapper = new PropertyMapper(optionsProvider);
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
            var mapper = new PropertyMapper(optionsProvider);
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
            var mapper = new PropertyMapper(optionsProvider);
            mapper.Property<Post>(p => p.Id).IsSortable().IsDefaultSort();

            // Act
            var metadata = mapper.GetDefaultMetadata<Post>();

            // Assert
            metadata.Name.Should().Be(nameof(Post.Id));
            metadata.IsDefaultSorting.Should().BeTrue();
            metadata.PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }

        [Fact]
        public void Mapper_Returns_AllMetadata()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new PropertyMapper(optionsProvider);
            mapper.Property<Post>(p => p.Id).IsSortable();
            mapper.Property<Exception>(p => p.Message).IsSortable();

            // Act
            var metadatas = mapper.GetAllMetadata();

            // Assert
            metadatas.Should().HaveCount(2);
            metadatas.First().Value.Should().HaveCount(1);
            metadatas.Last().Value.Should().HaveCount(1);
        }

        [Fact]
        public void Mapper_Returns_AllMetadata_For_SpecificType()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new PropertyMapper(optionsProvider);
            mapper.Property<Post>(p => p.Id).IsSortable();
            mapper.Property<Exception>(p => p.Message).IsSortable();

            // Act
            var metadatas = mapper.GetAllMetadata<Post>();
            var metadata = metadatas.FirstOrDefault();

            // Assert
            metadatas.Should().HaveCount(1);
            metadata.Name.Should().Be(nameof(Post.Id));
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
            var mapper = new PropertyMapper(optionsProvider);
            var metadata = new PropertyMetadata()
            {
                DisplayName = nameof(Post.Id),
                Name = nameof(Post.Id),
                PropertyInfo = typeof(Post).GetProperty(nameof(Post.Id)),
            };

            // Act
            mapper.AddMetadata<Post>(metadata);
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
            var mapper = new PropertyMapper(optionsProvider);
            var metadata1 = new PropertyMetadata()
            {
                DisplayName = nameof(Post.Id),
                Name = nameof(Post.Id),
                PropertyInfo = typeof(Post).GetProperty(nameof(Post.Id)),
            };
            var metadata2 = new PropertyMetadata()
            {
                DisplayName = nameof(Post.Id),
                IsFilterable = true,
                Name = nameof(Post.Id),
                PropertyInfo = typeof(Post).GetProperty(nameof(Post.Id)),
            };

            // Act
            mapper.AddMetadata<Post>(metadata1);
            mapper.AddMetadata<Post>(metadata2);
            var metadatas = mapper.GetAllMetadata<Post>();

            // Assert
            metadatas.Should().HaveCount(2);
            metadatas.First().IsFilterable.Should().BeFalse();
            metadatas.Last().IsFilterable.Should().BeTrue();
        }

        [Fact]
        public void Mapper_Adds_AlreadyExistingMaps_Via_AddMap_Without_Duplicating()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new PropertyMapper(optionsProvider);
            var metadata = new PropertyMetadata()
            {
                DisplayName = nameof(Post.Id),
                Name = nameof(Post.Id),
                PropertyInfo = typeof(Post).GetProperty(nameof(Post.Id)),
            };

            // Act
            mapper.AddMetadata<Post>(metadata);
            mapper.AddMetadata<Post>(metadata);
            var metadatas = mapper.GetAllMetadata<Post>();

            // Assert
            metadatas.Should().HaveCount(1);
            metadatas.First().Name.Should().Be(nameof(Post.Id));
            metadatas.First().PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }

        [Fact]
        public void Mapper_Property_Throws_Exception_With_FluentApiMetadataSourceType_Disabled()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions { MetadataSourceType = MetadataSourceType.Attributes });
            var optionsProvider = optionsMock.Object;
            var mapper = new PropertyMapper(optionsProvider);

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
            var mapper = new PropertyMapper(optionsProvider);
            var metadata = new PropertyMetadata()
            {
                DisplayName = nameof(Post.Id),
                Name = nameof(Post.Id),
                PropertyInfo = typeof(Post).GetProperty(nameof(Post.Id)),
            };

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => mapper.AddMetadata<Post>(metadata));
        }

        [Fact]
        public void Mapper_FindProperty_Throws_Exception_With_FluentApiMetadataSourceType_Disabled()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions { MetadataSourceType = MetadataSourceType.Attributes });
            var optionsProvider = optionsMock.Object;
            var mapper = new PropertyMapper(optionsProvider);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => mapper.GetMetadata<Post>(true, true, null));
        }

        private class Post
        {
            public int Id { get; set; }
        }
    }
}
