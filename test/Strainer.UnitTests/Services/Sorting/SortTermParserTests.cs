using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Sorting;
using Moq;

namespace Fluorite.Strainer.UnitTests.Services.Sorting
{
    public class SortTermParserTests
    {
        private readonly Mock<IStrainerOptionsProvider> _strainerOptionsProviderMock = new();
        private readonly Mock<ISortingWayFormatter> _sortingWayFormatterMock = new();
        private readonly Mock<ISortTermValueParser> _sortTermValueParserMock = new();

        private readonly SortTermParser _parser;

        public SortTermParserTests()
        {
            _parser = new SortTermParser(
                _sortingWayFormatterMock.Object,
                _strainerOptionsProviderMock.Object,
                _sortTermValueParserMock.Object);
        }

        [Theory]
        [InlineData(null)]
        [InlineData("")]
        public void Parser_Returns_NoSortTerm_When_InputIsNullOrEmpty(string input)
        {
            // Act
            var sortTermList = _parser.GetParsedTerms(input);

            // Assert
            sortTermList.Should().NotBeNull();
            sortTermList.Should().BeEmpty();
        }

        [Fact]
        public void Parser_Returns_NoSortTerms_WhenValueParserReturnsNoValues()
        {
            // Arrange
            var input = " ";

            _sortTermValueParserMock
                .Setup(x => x.GetParsedValues(input))
                .Returns(Array.Empty<string>());

            // Act
            var sortTermList = _parser.GetParsedTerms(input);

            // Assert
            sortTermList.Should().NotBeNull();
            sortTermList.Should().BeEmpty();
        }

        [Fact]
        public void Parser_Returns_SortTerms()
        {
            // Arrange
            var input = "foo";
            var parsedInput = "parsed";
            var formattedValue = "bar";
            var sortingWay = SortingWay.Descending;

            _sortTermValueParserMock
                .Setup(x => x.GetParsedValues(input))
                .Returns(new[] { parsedInput });
            _sortingWayFormatterMock
                .Setup(x => x.GetSortingWay(parsedInput))
                .Returns(sortingWay);
            _sortingWayFormatterMock
                .Setup(x => x.Unformat(parsedInput, sortingWay))
                .Returns(formattedValue);
            _strainerOptionsProviderMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());

            // Act
            var sortTermList = _parser.GetParsedTerms(input);

            // Assert
            sortTermList.Should().NotBeNullOrEmpty();
            sortTermList.Should().HaveCount(1);
            sortTermList.First().Should().NotBeNull();
            sortTermList.First().Input.Should().Be(parsedInput);
            sortTermList.First().IsDescending.Should().Be(sortingWay == SortingWay.Descending);
            sortTermList.First().Name.Should().Be(formattedValue);
        }
    }
}
