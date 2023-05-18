using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.UnitTests.Services.Sorting
{
    public class SortTermValueParserTests
    {
        private readonly SortTermValueParser _parser;

        public SortTermValueParserTests()
        {
            _parser = new SortTermValueParser();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Return_EmptyList_ForNullOrEmptyInput(string input)
        {
            // Act
            var result = _parser.GetParsedValues(input);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Theory]
        [InlineData(" ", "")]
        [InlineData("foo", "foo")]
        [InlineData(" foo", "foo")]
        [InlineData("foo ", "foo")]
        [InlineData(" foo ", "foo")]
        [InlineData(",", "")]
        [InlineData(" ,", "")]
        [InlineData("foo,", "foo")]
        [InlineData(",bar", "")]
        [InlineData("foo,bar", "foo")]
        public void Should_Return_ParsedSortValues(string input, string firstValue)
        {
            // Act
            var result = _parser.GetParsedValues(input);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.First().Should().Be(firstValue);
        }
    }
}
