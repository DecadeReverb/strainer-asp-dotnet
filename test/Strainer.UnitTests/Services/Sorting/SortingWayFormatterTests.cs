using FluentAssertions;
using Fluorite.Strainer.Services.Sorting;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Sorting
{
    public class SortingWayFormatterTests
    {
        private readonly static string DescendingWaySortingPrefix = DescendingPrefixSortingWayFormatter.Prefix;

        [Fact]
        public void Formatter_DoesNotChangeInput_WhenInputIsEmpty()
        {
            // Arrange
            var input = string.Empty;
            var isDescending = false;
            ISortingWayFormatter formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.Format(input, isDescending);

            // Assert
            result
                .Should()
                .BeEmpty();
        }

        [Fact]
        public void Formatter_DoesNotChangeInput_WhenInputIsWhitespace()
        {
            // Arrange
            var input = " ";
            var isDescending = false;
            ISortingWayFormatter formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.Format(input, isDescending);

            // Assert
            result
                .Should()
                .BeNullOrWhiteSpace();
        }

        [Fact]
        public void Formatter_AddsDescendingPrefix_IfSortingWayIsDescending()
        {
            // Arrange
            var input = "foo";
            var isDescending = true;
            ISortingWayFormatter formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.Format(input, isDescending);

            // Assert
            result
                .Should()
                .NotBe(input);
        }

        [Fact]
        public void Formatter_DoesNotAddDescendingPrefix_IfSortingWayIsAscending()
        {
            // Arrange
            var input = "foo";
            var isDescending = false;
            ISortingWayFormatter formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.Format(input, isDescending);

            // Assert
            result
                .Should()
                .Be(input);
        }

        [Fact]
        public void Formatter_ChecksSortingWay_Ascending()
        {
            // Arrange
            var input = "foo";
            ISortingWayFormatter formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.IsDescending(input);

            // Assert
            result
                .Should()
                .BeFalse();
        }

        [Fact]
        public void Formatter_ChecksSortingWay_Descending()
        {
            // Arrange
            var input = DescendingWaySortingPrefix + "foo";
            ISortingWayFormatter formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.IsDescending(input);

            // Assert
            result
                .Should()
                .BeTrue();
        }

        [Fact]
        public void Formatter_UnformatsInput_RemovingThePrefix()
        {
            // Arrange
            var input = DescendingWaySortingPrefix + "foo";
            ISortingWayFormatter formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.Unformat(input);

            // Assert
            result
                .Should()
                .Be("foo");
        }

        [Fact]
        public void Formatter_UnformatsInput_RemovingThePrefix_EvenForOnlyPrefix()
        {
            // Arrange
            var input = DescendingWaySortingPrefix;
            ISortingWayFormatter formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.Unformat(input);

            // Assert
            result
                .Should()
                .Be(string.Empty);
        }

        [Fact]
        public void Formatter_UnformatsInput_WithoutChanges()
        {
            // Arrange
            var input = "foo";
            ISortingWayFormatter formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.Unformat(input);

            // Assert
            result
                .Should()
                .Be(input);
        }

        [Fact]
        public void Formatter_UnformatsInput_WithoutChanges_ForEmptyString()
        {
            // Arrange
            var input = string.Empty;
            ISortingWayFormatter formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.Unformat(input);

            // Assert
            result
                .Should()
                .Be(input);
        }

        [Fact]
        public void Formatter_UnformatsInput_WithoutChanges_ForWhitespaceString()
        {
            // Arrange
            var input = " ";
            ISortingWayFormatter formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.Unformat(input);

            // Assert
            result
                .Should()
                .Be(input);
        }
    }
}
