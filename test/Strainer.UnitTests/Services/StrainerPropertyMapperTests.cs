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
            var (propertyName, propertyInfo) = mapper.FindProperty<Post>(
                canSortRequired: false,
                canFilterRequired: false,
                name: nameof(Post.Id),
                isCaseSensitive: true);

            // Assert
            propertyName.Should().BeNull();
            propertyInfo.Should().BeNull();
        }

        [Fact]
        public void Mapper_Returns_Map_WhenMarkedAsFilterable()
        {
            // Arrange
            var mapper = new StrainerPropertyMapper();

            // Act
            mapper.Property<Post>(p => p.Id)
                .CanFilter();
            var (propertyName, propertyInfo) = mapper.FindProperty<Post>(
                canSortRequired: false,
                canFilterRequired: false,
                name: nameof(Post.Id),
                isCaseSensitive: true);

            // Assert
            propertyName.Should().Be(nameof(Post.Id));
            propertyInfo.Should().BeSameAs(typeof(Post).GetProperty(propertyName));
        }

        [Fact]
        public void Mapper_Returns_Map_WhenMarkedAsSortable()
        {
            // Arrange
            var mapper = new StrainerPropertyMapper();

            // Act
            mapper.Property<Post>(p => p.Id)
                .CanSort();
            var (propertyName, propertyInfo) = mapper.FindProperty<Post>(
                canSortRequired: false,
                canFilterRequired: false,
                name: nameof(Post.Id),
                isCaseSensitive: true);

            // Assert
            propertyName.Should().Be(nameof(Post.Id));
            propertyInfo.Should().BeSameAs(typeof(Post).GetProperty(propertyName));
        }

        [Fact]
        public void Mapper_Adds_Map()
        {
            // Arrange
            var mapper = new StrainerPropertyMapper();
            var metadata = new StrainerPropertyMetadata()
            {
                DisplayName = nameof(Post.Id),
                Name = nameof(Post.Id),
            };
            var idPropertyInfo = typeof(Post).GetProperty(nameof(Post.Id));

            // Act
            mapper.AddMap<Post>(idPropertyInfo, metadata);
            var (propertyName, propertyInfo) = mapper.FindProperty<Post>(
                canSortRequired: false,
                canFilterRequired: false,
                name: nameof(Post.Id),
                isCaseSensitive: true);

            // Assert
            propertyName.Should().Be(nameof(Post.Id));
            propertyInfo.Should().BeSameAs(idPropertyInfo);
        }

        [Fact]
        public void Mapper_Adds_AlreadyExistingMaps()
        {
            // Arrange
            var mapper = new StrainerPropertyMapper();
            var metadata = new StrainerPropertyMetadata()
            {
                DisplayName = nameof(Post.Id),
                Name = nameof(Post.Id),
            };
            var idPropertyInfo = typeof(Post).GetProperty(nameof(Post.Id));

            // Act
            mapper.AddMap<Post>(idPropertyInfo, metadata);
            mapper.AddMap<Post>(idPropertyInfo, metadata);
            var (propertyName, propertyInfo) = mapper.FindProperty<Post>(
                canSortRequired: false,
                canFilterRequired: false,
                name: nameof(Post.Id),
                isCaseSensitive: true);

            // Assert
            propertyName.Should().Be(nameof(Post.Id));
            propertyInfo.Should().BeSameAs(idPropertyInfo);
        }
    }
}
