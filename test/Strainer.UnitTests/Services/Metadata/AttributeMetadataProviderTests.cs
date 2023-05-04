﻿using Fluorite.Extensions.DependencyInjection;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Reflection;

namespace Fluorite.Strainer.UnitTests.Services.Metadata
{
    public class AttributeMetadataProviderTests
    {
        private readonly Mock<IMetadataSourceTypeProvider> _metadataSourceTypeProviderMock = new();
        private readonly Mock<IStrainerOptionsProvider> _strainerOptionsProviderMock = new();
        private readonly Mock<IMetadataAssemblySourceProvider> _metadataAssemblySourceProviderMock = new();
        private readonly Mock<IObjectMetadataProvider> _objectMetadataProviderMock = new();

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
            var attributeMetadataProvider = BuildMetadataProvider();

            // Act
            var result = attributeMetadataProvider.GetAllPropertyMetadata();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().HaveCount(2);
            result.Keys.Should().BeEquivalentTo(types);
        }

        [Fact]
        public void Provider_Returns_DefaultMetadata()
        {
            // Arrange
            _strainerOptionsProviderMock
                .Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            _objectMetadataProviderMock
                .Setup(x => x.GetDefaultMetadataFromObjectAttribute(typeof(Comment)))
                .Returns(Mock.Of<IPropertyMetadata>());
            var attributeMetadataProvider = BuildMetadataProvider();

            // Act
            var result = attributeMetadataProvider.GetDefaultMetadata<Comment>();

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void Provider_Returns_NoPropertyMetadata_WhenNoneAreMatchingConditions()
        {
            // Arrange
            _strainerOptionsProviderMock
                .Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var attributeMetadataProvider = BuildMetadataProvider();

            // Act
            var result = attributeMetadataProvider.GetPropertyMetadata<Post>(
                isSortableRequired: true,
                isFilterableRequired: true,
                name: nameof(Post.Id));

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Provider_Returns_NoPropertyMetadata_WhenAttributeMetadataSource_Is_Disabled()
        {
            // Arrange
            _strainerOptionsProviderMock
                .Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions
                {
                    MetadataSourceType = MetadataSourceType.FluentApi,
                });
            var attributeMetadataProvider = BuildMetadataProvider();

            // Act
            var result = attributeMetadataProvider.GetPropertyMetadata<Post>(
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
            _strainerOptionsProviderMock
                .Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var attributeMetadataProvider = BuildMetadataProvider();

            // Act
            var result = attributeMetadataProvider.GetPropertyMetadata<Post>(
                isSortableRequired: true,
                isFilterableRequired: true,
                name: nameof(Post.Title));

            // Assert
            result.Should().NotBeNull();
            result.IsFilterable.Should().BeTrue();
            result.IsSortable.Should().BeTrue();
        }

        [Fact]
        public void Provider_Returns_PropertyMetadata_FromStrainerPropertyAttribute()
        {
            // Arrange
            _strainerOptionsProviderMock
                .Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var attributeMetadataProvider = BuildMetadataProvider();

            // Act
            var result = attributeMetadataProvider.GetPropertyMetadata<Post>(
                isSortableRequired: true,
                isFilterableRequired: true,
                name: nameof(Post.Title));

            // Assert
            result.Should().NotBeNull();
            result.IsFilterable.Should().BeTrue();
            result.IsSortable.Should().BeTrue();
        }

        [Fact]
        public void Provider_Returns_PropertyMetadata_FromStrainerObjectAttribute()
        {
            // Arrange
            _strainerOptionsProviderMock
                .Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var attributeMetadataProvider = BuildMetadataProvider();

            // Act
            var result = attributeMetadataProvider.GetPropertyMetadata<Comment>(
                isSortableRequired: true,
                isFilterableRequired: true,
                name: nameof(Comment.Id));

            // Assert
            result.Should().NotBeNull();
            result.IsFilterable.Should().BeTrue();
            result.IsSortable.Should().BeTrue();
        }

        [Fact]
        public void AttributeMetadataProvider_Works_For_Object_Attribute()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddStrainer();
            services.AddScoped<AttributeMetadataProvider>();
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var attributeMetadataProvider = serviceProvider.GetRequiredService<AttributeMetadataProvider>();
            var metadatas = attributeMetadataProvider.GetPropertyMetadatas<Comment>();

            // Assert
            metadatas.Should().NotBeNullOrEmpty();
            metadatas.Should().HaveCount(1);
            metadatas.First().Name.Should().Be(nameof(Comment.Id));
        }

        [Fact]
        public void AttributeMetadataProvider_Works_For_Property_Attribute()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddStrainer();
            services.AddScoped<AttributeMetadataProvider>();
            var serviceProvider = services.BuildServiceProvider();

            // Act
            var attributeMetadataProvider = serviceProvider.GetRequiredService<AttributeMetadataProvider>();
            var metadatas = attributeMetadataProvider.GetPropertyMetadatas<Post>();

            // Assert
            metadatas.Should().NotBeNullOrEmpty();
            metadatas.Should().HaveCount(1);
            metadatas.First().Should().BeAssignableTo<StrainerPropertyAttribute>();
            metadatas.First().Name.Should().Be(nameof(Post.Title));
        }

        private AttributeMetadataProvider BuildMetadataProvider()
        {
            return new AttributeMetadataProvider(
                _strainerOptionsProviderMock.Object,
                _metadataSourceTypeProviderMock.Object,
                _metadataAssemblySourceProviderMock.Object,
                _objectMetadataProviderMock.Object);
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
