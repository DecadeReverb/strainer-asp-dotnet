using Fluorite.Strainer.Services.Filtering;

namespace Fluorite.Strainer.UnitTests.Services.Filtering;

public class FilterTermNamesParserTests
{
    private readonly FilterTermNamesParser _parser;

    public FilterTermNamesParserTests()
    {
        _parser = new FilterTermNamesParser();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    public void Should_Return_EmptyList_ForNullOrEmptyInput(string input)
    {
        // Act
        var result = _parser.Parse(input);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData("|")]
    public void Should_Return_EscapedFilterTermNames_ButWithoutNullEmptyOrWhiteSpaceNames(string input)
    {
        // Act
        var result = _parser.Parse(input);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }

    [Theory]
    [InlineData("//", "//")]
    [InlineData("foo", "foo")]
    [InlineData("foo|bar", "foo", "bar")]
    [InlineData("(foo|bar)", "(foo", "bar)")]
    [InlineData("(foo)|(bar)", "(foo)", "(bar)")]
    [InlineData("(foo|bar)|(aaa)", "(foo", "bar)", "(aaa)")]
    public void Should_Return_EscapedFilterTermNames(string input, params string[] expectedNames)
    {
        // Act
        var result = _parser.Parse(input);

        // Assert
        result.Should().BeEquivalentTo(expectedNames);
    }
}
