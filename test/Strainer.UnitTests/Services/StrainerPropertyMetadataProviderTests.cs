using FluentAssertions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.TestModels;
using Moq;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services
{
    public class StrainerPropertyMetadataProviderTests
    {
        [Fact]
        public void Provider_Returns_NoPropertyMetadata_WhenRegistredWithFluentApi()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new PropertyMapper(optionsProvider);
            var attributeMetadataProvider = new AttributePropertyMetadataProvider(mapper, optionsProvider);

            // Act
            var result = attributeMetadataProvider.GetPropertyMetadata<Post>(
                isSortingRequired: false,
                isFilteringRequired: true,
                name: nameof(Post.Id));

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Provider_Returns_NoPropertyMetadata_WhenNoneAreMatchingConditions()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var mapper = new PropertyMapper(optionsProvider);
            var attributeMetadataProvider = new AttributePropertyMetadataProvider(mapper, optionsProvider);

            // Act
            var result = attributeMetadataProvider.GetPropertyMetadata<Post>(
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
            var mapper = new PropertyMapper(optionsProvider);
            var attributeMetadataProvider = new AttributePropertyMetadataProvider(mapper, optionsProvider);

            // Act
            var result = attributeMetadataProvider.GetPropertyMetadata<Post>(
                isSortingRequired: false,
                isFilteringRequired: true,
                name: nameof(Post.Title));

            // Assert
            result.Should().NotBeNull();
            result.IsFilterable.Should().BeTrue();
            result.IsSortable.Should().BeTrue();
        }
    }
}
