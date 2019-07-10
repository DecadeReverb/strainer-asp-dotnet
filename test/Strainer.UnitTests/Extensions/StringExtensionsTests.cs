using FluentAssertions;
using Fluorite.Extensions;
using System;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Extensions
{
    public class StringExtensionsTests
    {
        [Fact]
        public void Extension_Checks_IfStringIsAPartOfOtherString()
        {
            // Arrange
            var value = "foo";
            var subValue = "f";

            // Act
            var result = StringExtensions.Contains(value, subValue, StringComparison.OrdinalIgnoreCase);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Extension_Checks_IfStringIsAPartOfOtherString_WorksWithEmptyStrings()
        {
            // Arrange
            var value = string.Empty;
            var subValue = string.Empty;

            // Act
            var result = StringExtensions.Contains(value, subValue, StringComparison.OrdinalIgnoreCase);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void Extension_ThrowsException_WhenCurrentInstanceIsNull()
        {
            // Arrange
            string value = null;
            var subValue = "foo";

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => StringExtensions.Contains(value, subValue, StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void Extension_ThrowsException_WhenArgumentInstanceIsNull()
        {
            // Arrange
            var value = "foo";
            string subValue = null;

            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => StringExtensions.Contains(value, subValue, StringComparison.OrdinalIgnoreCase));
        }
    }
}
