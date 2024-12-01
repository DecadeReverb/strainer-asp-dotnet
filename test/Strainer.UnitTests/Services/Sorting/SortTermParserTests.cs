using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.UnitTests.Services.Sorting;

public class SortTermParserTests
{
    private readonly IStrainerOptionsProvider _strainerOptionsProviderMock = Substitute.For<IStrainerOptionsProvider>();
    private readonly ISortingWayFormatter _sortingWayFormatterMock = Substitute.For<ISortingWayFormatter>();
    private readonly ISortTermValueParser _sortTermValueParserMock = Substitute.For<ISortTermValueParser>();

    private readonly SortTermParser _parser;

    public SortTermParserTests()
    {
        _parser = new SortTermParser(
            _sortingWayFormatterMock,
            _strainerOptionsProviderMock,
            _sortTermValueParserMock);
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
            .GetParsedValues(input)
            .Returns([]);

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
            .GetParsedValues(input)
            .Returns([parsedInput]);
        _sortingWayFormatterMock
            .GetSortingWay(parsedInput)
            .Returns(sortingWay);
        _sortingWayFormatterMock
            .Unformat(parsedInput, sortingWay)
            .Returns(formattedValue);
        _strainerOptionsProviderMock
            .GetStrainerOptions()
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
