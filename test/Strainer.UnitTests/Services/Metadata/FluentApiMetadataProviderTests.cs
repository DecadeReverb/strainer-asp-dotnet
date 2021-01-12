using FluentAssertions;
using Fluorite.Extensions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Configuration;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Configuration;
using Fluorite.Strainer.Services.Metadata;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
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
            var propertyInfoProviderMock = new Mock<IPropertyInfoProvider>();
            var mapper = new MetadataMapper(optionsProvider, propertyInfoProviderMock.Object);

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
            var propertyInfoProviderMock = new Mock<IPropertyInfoProvider>();
            var mapper = new MetadataMapper(optionsProvider, propertyInfoProviderMock.Object);

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
            var propertyInfoProviderMock = new Mock<IPropertyInfoProvider>();
            var mapper = new MetadataMapper(optionsProvider, propertyInfoProviderMock.Object);

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
            var propertyInfoProvider = new PropertyInfoProvider();
            var mapper = new MetadataMapper(optionsProvider, propertyInfoProvider);

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
            var propertyInfoProviderMock = new Mock<IPropertyInfoProvider>();
            var mapper = new MetadataMapper(optionsProvider, propertyInfoProviderMock.Object);

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
            var propertyInfoProvider = new PropertyInfoProvider();
            var mapper = new MetadataMapper(optionsProvider, propertyInfoProvider);

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
            var propertyInfoProvider = new PropertyInfoProvider();
            var mapper = new MetadataMapper(optionsProvider, propertyInfoProvider);

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
            var propertyInfoProviderMock = new Mock<IPropertyInfoProvider>();
            var mapper = new MetadataMapper(optionsProvider, propertyInfoProviderMock.Object);

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
            var propertyInfoProviderMock = new Mock<IPropertyInfoProvider>();
            var mapper = new MetadataMapper(optionsProvider, propertyInfoProviderMock.Object);

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
            var propertyInfoProviderMock = new Mock<IPropertyInfoProvider>();
            var mapper = new MetadataMapper(optionsProvider, propertyInfoProviderMock.Object);

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

        [Fact]
        public void GetPropertyMetadata_Returns_Null_With_FluentApiMetadataSourceType_Disabled()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions { MetadataSourceType = MetadataSourceType.Attributes });
            var optionsProvider = optionsMock.Object;
            var propertyInfoProviderMock = new Mock<IPropertyInfoProvider>();
            var mapper = new MetadataMapper(optionsProvider, propertyInfoProviderMock.Object);
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
            var defaultMetadata = mapper.DefaultMetadata.ToReadOnly();
            var objectMetadata = mapper.ObjectMetadata.ToReadOnly();
            var propertyMetadata = mapper
                .PropertyMetadata
                .Select(pair =>
                    new KeyValuePair<Type, IReadOnlyDictionary<string, IPropertyMetadata>>(
                        pair.Key, pair.Value.ToReadOnly()))
                .ToReadOnlyDictionary();
            var strainerConfiguration = new StrainerConfiguration(
                new Dictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>(),
                new Dictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>>(),
                defaultMetadata,
                new Dictionary<string, IFilterOperator>(),
                objectMetadata,
                propertyMetadata);
            var strainerConfigurationProvider = new StrainerConfigurationProvider(strainerConfiguration);
            var configurationMetadataProvider = new ConfigurationMetadataProvider(strainerConfigurationProvider);
            var fluentApiMetadataProvider = new FluentApiMetadataProvider(
                optionsProvider,
                configurationMetadataProvider);

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
