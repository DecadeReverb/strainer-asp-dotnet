using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Services.Validation;

namespace Fluorite.Strainer.UnitTests.Services.Validation
{
    public class SortExpressionValidatorTests
    {
        private readonly SortExpressionValidator _validator;

        public SortExpressionValidatorTests()
        {
            _validator = new SortExpressionValidator();
        }

        [Fact]
        public void Should_Throw_ForNullMetadata()
        {
            // Act
            Action act = () => _validator.Validate(propertyMetadata: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Should_Throw_ForNoDefaultSortingProperty()
        {
            // Arrange
            var type = typeof(Version);
            var innerMetadata = new Dictionary<string, IPropertyMetadata>
            {
                { "foo", Substitute.For<IPropertyMetadata>() },
            };
            var propertyMetadata = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>
            {
                { type, innerMetadata },
            };

            // Act
            Action act = () => _validator.Validate(propertyMetadata);

            // Assert
            act.Should().ThrowExactly<StrainerSortExpressionValidatorException>()
                .WithMessage(
                    $"No default sort expression found for type {type.FullName}.\n" +
                    $"Mark a property as default sorting to enable fallbacking " +
                    $"to it when no sorting information is provided.")
                .Which.EntityType.Should().Be(type);
        }

        [Fact]
        public void Should_Throw_ForTooManyDefaultSortingProperties()
        {
            // Arrange
            var type = typeof(Version);
            var metadata1 = new PropertyMetadata
            {
                IsDefaultSorting = true,
                Name = "foo",
            };
            var metadata2 = new PropertyMetadata
            {
                IsDefaultSorting = true,
                Name = "bar",
            };
            var innerMetadata = new Dictionary<string, IPropertyMetadata>
            {
                { metadata1.Name, metadata1 },
                { metadata2.Name, metadata2 },
            };
            var propertyMetadata = new Dictionary<Type, IReadOnlyDictionary<string, IPropertyMetadata>>
            {
                { type, innerMetadata },
            };

            // Act
            Action act = () => _validator.Validate(propertyMetadata);

            // Assert
            act.Should().ThrowExactly<StrainerSortExpressionValidatorException>()
                .WithMessage(
                    $"Too many default sort properties found for type {type.FullName}.\n" +
                    $"Only one property can be marked as default.\n" +
                    $"Default properties:\n" +
                    $"{metadata1.Name}\n{metadata2.Name}")
                .Which.EntityType.Should().Be(type);
        }
    }
}
