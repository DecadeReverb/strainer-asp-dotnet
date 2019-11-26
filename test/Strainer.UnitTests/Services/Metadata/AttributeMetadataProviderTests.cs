using FluentAssertions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Metadata;
using Moq;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Metadata
{
    public class AttributeMetadataProviderTests
    {
        [Fact]
        public void Provider_Returns_DefaultMetadata()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var attributeMetadataProvider = new AttributeMetadataProvider(optionsProvider);

            // Act
            var result = attributeMetadataProvider.GetDefaultMetadata<Comment>();

            // Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void Provider_Returns_NoPropertyMetadata_WhenNoneAreMatchingConditions()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var attributeMetadataProvider = new AttributeMetadataProvider(optionsProvider);

            // Act
            var result = attributeMetadataProvider.GetMetadata<Post>(
                isSortingRequired: true,
                isFilteringRequired: true,
                name: nameof(Post.Id));

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Provider_Returns_NoPropertyMetadata_WhenAttributeMetadataSource_Is_Disabled()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions
                {
                    MetadataSourceType = MetadataSourceType.FluentApi,
                });
            var optionsProvider = optionsMock.Object;
            var attributeMetadataProvider = new AttributeMetadataProvider(optionsProvider);

            // Act
            var result = attributeMetadataProvider.GetMetadata<Post>(
                isSortingRequired: true,
                isFilteringRequired: true,
                name: nameof(Post.Id));

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Provider_Returns_PropertyMetadata_FromStrainerAttribute()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var attributeMetadataProvider = new AttributeMetadataProvider(optionsProvider);

            // Act
            var result = attributeMetadataProvider.GetMetadata<Post>(
                isSortingRequired: true,
                isFilteringRequired: true,
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
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var attributeMetadataProvider = new AttributeMetadataProvider(optionsProvider);

            // Act
            var result = attributeMetadataProvider.GetMetadata<Post>(
                isSortingRequired: true,
                isFilteringRequired: true,
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
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var attributeMetadataProvider = new AttributeMetadataProvider(optionsProvider);

            // Act
            var result = attributeMetadataProvider.GetMetadata<Comment>(
                isSortingRequired: true,
                isFilteringRequired: true,
                name: nameof(Comment.Id));

            // Assert
            result.Should().NotBeNull();
            result.IsFilterable.Should().BeTrue();
            result.IsSortable.Should().BeTrue();
        }

        private class Post
        {
            public int Id { get; set; }

            [StrainerProperty(IsFilterable = true, IsSortable = true)]
            public string Title { get; set; }
        }

        [StrainerObject(nameof(Id))]
        private class Comment
        {
            public int Id { get; set; }
        }
    }
}
