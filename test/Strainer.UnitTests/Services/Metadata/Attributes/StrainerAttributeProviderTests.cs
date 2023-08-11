using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Services.Metadata.Attributes;
using System.Reflection;

namespace Fluorite.Strainer.UnitTests.Services.Metadata.Attributes
{
    public class StrainerAttributeProviderTests
    {
        private readonly StrainerAttributeProvider _provider;

        public StrainerAttributeProviderTests()
        {
            _provider = new StrainerAttributeProvider();
        }

        [Fact]
        public void Should_Throw_WhenTypeIsNull()
        {
            // Act
            Action act = () => _provider.GetObjectAttribute(type: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Should_Return_NullWhen_ObjectAttributeIsNotFound()
        {
            // Arrange
            var type = typeof(Version);

            // Act
            var result = _provider.GetObjectAttribute(type);

            // Assert
            result.Should().BeNull();
        }

        [Fact]
        public void Should_Return_ObjectAttribute_WhenFound()
        {
            // Arrange
            var objectAttribute = new StrainerObjectAttribute("foo");
            var typeMock = Substitute.For<Type>();
            typeMock
                .GetCustomAttributes(typeof(StrainerObjectAttribute), false)
                .Returns(new[] { objectAttribute });

            // Act
            var result = _provider.GetObjectAttribute(typeMock);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeSameAs(objectAttribute);
        }

        [Fact]
        public void Should_Throw_WhenPropertyInfoIsNull()
        {
            // Act
            Action act = () => _provider.GetPropertyAttribute(propertyInfo: null);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Should_Return_PropertyAttribute_WhenFound()
        {
            // Arrange
            var propertyAttribute = new StrainerPropertyAttribute();
            var propertyInfoMock = Substitute.For<PropertyInfo>();
            propertyInfoMock
                .GetCustomAttributes(typeof(StrainerPropertyAttribute), false)
                .Returns(new[] { propertyAttribute });

            // Act
            var result = _provider.GetPropertyAttribute(propertyInfoMock);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeSameAs(propertyAttribute);
            result.PropertyInfo.Should().BeSameAs(propertyInfoMock);
        }

        [Fact]
        public void Should_Return_NullWhen_PropertyAttributeNotFound()
        {
            // Arrange
            var propertyInfoMock = Substitute.For<PropertyInfo>();

            // Act
            var result = _provider.GetPropertyAttribute(propertyInfoMock);

            // Assert
            result.Should().BeNull();
        }
    }
}
