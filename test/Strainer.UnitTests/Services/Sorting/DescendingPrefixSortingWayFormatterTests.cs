using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.UnitTests.Services.Sorting
{
    public class DescendingPrefixSortingWayFormatterTests
    {
        private const string DescendingPrefix = DescendingPrefixSortingWayFormatter.DescendingPrefix;

        [Fact]
        public void Formatter_Throws_ForNullInput_WhenFormatting()
        {
            // Arrange
            string input = null;
            var sortingWay = SortingWay.Ascending;
            var formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            Action act = () => formatter.Format(input, sortingWay);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Formatter_Throws_ForUnkownSortingWay_WhenFormatting()
        {
            // Arrange
            var input = string.Empty;
            var sortingWay = SortingWay.Unknown;
            var formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            Action act = () => formatter.Format(input, sortingWay);

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithMessage($"{nameof(sortingWay)} cannot be {nameof(SortingWay.Unknown)}.*");
        }

        [Theory]
        [InlineData("", SortingWay.Ascending, "")]
        [InlineData(" ", SortingWay.Ascending, " ")]
        [InlineData("foo", SortingWay.Ascending, "foo")]
        [InlineData("", SortingWay.Descending, "")]
        [InlineData(" ", SortingWay.Descending, " ")]
        [InlineData("foo", SortingWay.Descending, DescendingPrefix + "foo")]
        public void Formatter_AddsDescendingPrefix(string input, SortingWay sortingWay, string expectedResult)
        {
            // Arrange
            var formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.Format(input, sortingWay);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Fact]
        public void Formatter_Throws_ForNullInput_WhenGettingSortingWay()
        {
            // Arrange
            string input = null;
            var formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            Action act = () => formatter.GetSortingWay(input);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Theory]
        [InlineData("", SortingWay.Unknown)]
        [InlineData(" ", SortingWay.Unknown)]
        [InlineData("foo", SortingWay.Ascending)]
        [InlineData(DescendingPrefix + "foo", SortingWay.Descending)]
        public void Formatter_Returns_CorrectSortingWay(string input, SortingWay sortingWay)
        {
            // Arrange
            var formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.GetSortingWay(input);

            // Assert
            result.Should().Be(sortingWay);
        }

        [Fact]
        public void Formatter_Throws_ForNullInput_WhenUnformatting()
        {
            // Arrange
            string input = null;
            var sortingWay = SortingWay.Ascending;
            var formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            Action act = () => formatter.Unformat(input, sortingWay);

            // Assert
            act.Should().ThrowExactly<ArgumentNullException>();
        }

        [Fact]
        public void Formatter_Throws_ForUnkownSortingWay_WhenUnformatting()
        {
            // Arrange
            var input = string.Empty;
            var sortingWay = SortingWay.Unknown;
            var formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            Action act = () => formatter.Unformat(input, sortingWay);

            // Assert
            act.Should().ThrowExactly<ArgumentException>()
                .WithMessage($"{nameof(sortingWay)} cannot be {nameof(SortingWay.Unknown)}.*");
        }

        [Theory]
        [InlineData("", "")]
        [InlineData(" ", " ")]
        [InlineData(DescendingPrefix, "")]
        [InlineData(DescendingPrefix + "foo", "foo")]
        [InlineData("foo", "foo")]
        public void Formatter_Returns_UnformatedInput(string input, string expectedResult)
        {
            // Arrange
            var sortingWay = SortingWay.Descending;
            var formatter = new DescendingPrefixSortingWayFormatter();

            // Act
            var result = formatter.Unformat(input, sortingWay);

            // Assert
            result.Should().Be(expectedResult);
        }
    }
}
