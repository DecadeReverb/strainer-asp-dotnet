using Fluorite.Strainer.Services.Filtering;

namespace Fluorite.Strainer.UnitTests.Services.Filtering
{
    public class FilterTermValuesParserTests
    {
        private readonly FilterTermValuesParser _parser;

        public FilterTermValuesParserTests()
        {
            _parser = new FilterTermValuesParser();
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Should_Return_EmptyList(string input)
        {
            // Act
            var result = _parser.Parse(input);

            // Assert
            result.Should().NotBeNull();
            result.Should().BeEmpty();
        }

        [Theory]
        [InlineData(" ", "")]
        [InlineData(",", ",")]
        [InlineData("\\,", ",")]
        [InlineData("foo", "foo")]
        [InlineData("|", "")]
        [InlineData("foo|bar", "foo")]
        [InlineData("foo\\,bar|lorem\\,ipsum", "foo,bar")]
        public void Should_Return_SplittedValues(string input, string firstValue)
        {
            // Act
            var result = _parser.Parse(input);

            // Assert
            result.Should().NotBeNull();
            result.Should().NotBeEmpty();
            result.First().Should().Be(firstValue);
        }
    }
}
