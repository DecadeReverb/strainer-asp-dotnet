﻿using FluentAssertions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.TestModels;
using Moq;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services
{
    public class StrainerPropertyMapperTests
    {
        [Fact]
        public void Mapper_Returns_Null_WhenJustPropertyIsCalled()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new PropertyMapper(optionsProvider);

            // Act
            mapper.Property<Post>(p => p.Id);
            var metadata = mapper.FindProperty<Post>(
                isSortableRequired: false,
                isFilterableRequired: false,
                name: nameof(Post.Id));

            // Assert
            metadata.Should().BeNull();
        }

        [Fact]
        public void Mapper_Returns_Map_WhenMarkedAsFilterable()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new PropertyMapper(optionsProvider);

            // Act
            mapper.Property<Post>(p => p.Id)
                .IsFilterable();
            var metadata = mapper.FindProperty<Post>(
                isSortableRequired: false,
                isFilterableRequired: false,
                name: nameof(Post.Id));

            // Assert
            metadata.Name.Should().Be(nameof(Post.Id));
            metadata.PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }

        [Fact]
        public void Mapper_Returns_Map_WhenMarkedAsSortable()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new PropertyMapper(optionsProvider);

            // Act
            mapper.Property<Post>(p => p.Id)
                .IsSortable();
            var metadata = mapper.FindProperty<Post>(
                isSortableRequired: false,
                isFilterableRequired: false,
                name: nameof(Post.Id));

            // Assert
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
            mapper.AddMap<Post>(metadata);
            var result = mapper.FindProperty<Post>(
                isSortableRequired: false,
                isFilterableRequired: false,
                name: nameof(Post.Id));

            // Assert
            result.Name.Should().Be(nameof(Post.Id));
            result.PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }

        [Fact]
        public void Mapper_Adds_AlreadyExistingMaps_Via_AddMap()
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
            var idPropertyInfo = typeof(Post).GetProperty(nameof(Post.Id));

            // Act
            mapper.AddMap<Post>(metadata);
            mapper.AddMap<Post>(metadata);
            var result = mapper.FindProperty<Post>(
                isSortableRequired: false,
                isFilterableRequired: false,
                name: nameof(Post.Id));

            // Assert
            result.Name.Should().Be(nameof(Post.Id));
            result.PropertyInfo.Should().BeSameAs(typeof(Post).GetProperty(metadata.Name));
        }
    }
}
