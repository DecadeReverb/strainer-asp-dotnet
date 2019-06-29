using FluentAssertions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.UnitTests.Entities;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services
{
    public class StrainerPropertyMapperTests
    {
        [Fact]
        public void Mapper_Returns_Null_WhenJustPropertyIsCalled()
        {
            // Arrange
            var mapper = new StrainerPropertyMapper();

            // Act
            mapper.Property<Post>(p => p.Id);
            var metadata = mapper.FindProperty<Post>(
                canSortRequired: false,
                canFilterRequired: false,
                name: nameof(Post.Id),
                isCaseSensitive: true);

            // Assert
            metadata.Should().BeNull();
        }

        [Fact]
        public void Mapper_Returns_Map_WhenMarkedAsFilterable()
        {
            // Arrange
            var mapper = new StrainerPropertyMapper();

            // Act
            mapper.Property<Post>(p => p.Id)
                .CanFilter();
            var metadata = mapper.FindProperty<Post>(
                canSortRequired: false,
                canFilterRequired: false,
                name: nameof(Post.Id),
                isCaseSensitive: true);

            // Assert
            metadata.Name.Should().Be(nameof(Post.Id));
            metadata.PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }

        [Fact]
        public void Mapper_Returns_Map_WhenMarkedAsSortable()
        {
            // Arrange
            var mapper = new StrainerPropertyMapper();

            // Act
            mapper.Property<Post>(p => p.Id)
                .CanSort();
            var metadata = mapper.FindProperty<Post>(
                canSortRequired: false,
                canFilterRequired: false,
                name: nameof(Post.Id),
                isCaseSensitive: true);

            // Assert
            metadata.Name.Should().Be(nameof(Post.Id));
            metadata.PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }

        [Fact]
        public void Mapper_Adds_Map_Via_AddMap()
        {
            // Arrange
            var mapper = new StrainerPropertyMapper();
            var metadata = new StrainerPropertyMetadata()
            {
                DisplayName = nameof(Post.Id),
                Name = nameof(Post.Id),
                PropertyInfo = typeof(Post).GetProperty(nameof(Post.Id)),
            };

            // Act
            mapper.AddMap<Post>(metadata);
            var result = mapper.FindProperty<Post>(
                canSortRequired: false,
                canFilterRequired: false,
                name: nameof(Post.Id),
                isCaseSensitive: true);

            // Assert
            result.Name.Should().Be(nameof(Post.Id));
            result.PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }

        [Fact]
        public void Mapper_Adds_AlreadyExistingMaps_Via_AddMap()
        {
            // Arrange
            var mapper = new StrainerPropertyMapper();
            var metadata = new StrainerPropertyMetadata()
            {
                DisplayName = nameof(Post.Id),
                Name = nameof(Post.Id),
                PropertyInfo = typeof(Post).GetProperty(nameof(Post.Id)),
            };
            var idPropertyInfo = typeof(Post).GetProperty(nameof(Post.Id));

            // Act
            mapper.AddMap<Post>(metadata);
            mapper.AddMap<Post>(metadata);
            var result = mapper.FindProperty<Post>(
                canSortRequired: false,
                canFilterRequired: false,
                name: nameof(Post.Id),
                isCaseSensitive: true);

            // Assert
            result.Name.Should().Be(nameof(Post.Id));
            result.PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }
    }
}
