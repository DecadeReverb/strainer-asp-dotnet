using FluentAssertions;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services.Sorting;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Sorting
{
    public class DescendingPrefixSortingWayFormatterTests
    {
        private readonly static string DescendingPrefix = DescendingPrefixSortingWayFormatter.DescendingPrefix;

        [Fact]
        public void Formatter_DoesNot_ChangeInput_When_Input_Is_Empty()
        {
            // Arrange
            var input = string.Empty;
            var sortingWay = SortingWay.Descending;
            var formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.Format(input, sortingWay);

            // Assert
            result.Should().BeEmpty();
        }

        [Fact]
        public void Formatter_DoesNot_Change_Input_When_Input_Is_Whitespace()
        {
            // Arrange
            var input = " ";
            var sortingWay = SortingWay.Ascending;
            var formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.Format(input, sortingWay);

            // Assert
            result.Should().BeNullOrWhiteSpace();
        }

        [Fact]
        public void Formatter_AddsDescendingPrefix_If_SortingWay_Is_Descending()
        {
            // Arrange
            var input = "foo";
            var sortingWay = SortingWay.Descending;
            var formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.Format(input, sortingWay);

            // Assert
            result.Should().NotBe(input);
            result.Should().Be(DescendingPrefix + input);
        }

        [Fact]
        public void Formatter_DoesNot_Add_DescendingPrefix_If_SortingWay_Is_Ascending()
        {
            // Arrange
            var input = "foo";
            var sortingWay = SortingWay.Ascending;
            var formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.Format(input, sortingWay);

            // Assert
            result.Should().Be(input);
        }

        [Fact]
        public void Formatter_Gets_SortingWay_Ascending()
        {
            // Arrange
            var input = "foo";
            var formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.GetSortingWay(input);

            // Assert
            result.Should().Be(SortingWay.Ascending);
        }

        [Fact]
        public void Formatter_Gets_SortingWay_Descending()
        {
            // Arrange
            var input = DescendingPrefix + "foo";
            var formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.GetSortingWay(input);

            // Assert
            result.Should().Be(SortingWay.Descending);
        }

        [Fact]
        public void Formatter_Unformats_Input_Removing_Prefix()
        {
            // Arrange
            var input = DescendingPrefix + "foo";
            var sortingWay = SortingWay.Descending;
            var formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.Unformat(input, sortingWay);

            // Assert
            result.Should().Be("foo");
        }

        [Fact]
        public void Formatter_Unformats_Input_Removing_Prefix_Even_For_Only_Prefix()
        {
            // Arrange
            var input = DescendingPrefix;
            var sortingWay = SortingWay.Descending;
            var formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.Unformat(input, sortingWay);

            // Assert
            result.Should().Be(string.Empty);
        }

        [Fact]
        public void Formatter_Unformats_Input_Without_Changes()
        {
            // Arrange
            var input = "foo";
            var sortingWay = SortingWay.Ascending;
            var formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.Unformat(input, sortingWay);

            // Assert
            result.Should().Be(input);
        }

        [Fact]
        public void Formatter_Unformats_Input_Without_Changes_For_EmptyString()
        {
            // Arrange
            var input = string.Empty;
            var sortingWay = SortingWay.Ascending;
            var formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.Unformat(input, sortingWay);

            // Assert
            result.Should().Be(input);
        }

        [Fact]
        public void Formatter_Unformats_Input_Without_Changes_For_WhitespaceString()
        {
            // Arrange
            var input = " ";
            var sortingWay = SortingWay.Ascending;
            var formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.Unformat(input, sortingWay);

            // Assert
            result.Should().Be(input);
        }
    }
}
