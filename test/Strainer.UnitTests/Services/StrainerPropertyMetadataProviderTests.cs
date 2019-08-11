using FluentAssertions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.TestModels;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services
{
    public class StrainerPropertyMetadataProviderTests
    {
        [Fact]
        public void Provider_Returns_NoPropertyMetadata_WhenRegistredWithFluentApi()
        {
            // Arrange
            var options = new StrainerOptions();
            var mapper = new PropertyMapper(options);
            var provider = new AttributePropertyMetadataProvider(mapper, options);

            // Act
            var result = provider.GetPropertyMetadata<Post>(
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
            var options = new StrainerOptions();
            var mapper = new PropertyMapper(options);
            var provider = new AttributePropertyMetadataProvider(mapper, options);

            // Act
            var result = provider.GetPropertyMetadata<Post>(
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
            var options = new StrainerOptions();
            var mapper = new PropertyMapper(options);
            var provider = new AttributePropertyMetadataProvider(mapper, options);

            // Act
            var result = provider.GetPropertyMetadata<Post>(
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
