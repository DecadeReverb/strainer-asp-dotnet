using FluentAssertions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Sorting;
using Moq;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Sorting
{
    public class SortTermParserTests
    {
        [Fact]
        public void Parser_Returns_NoSortTerm_When_Input_Is_Null()
        {
            // Arrange
            string input = null;
            var optionsProviderMock = new Mock<IStrainerOptionsProvider>();
            optionsProviderMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var formatter = new DescendingPrefixSortingWayFormatter();
            var parser = new SortTermParser(formatter, optionsProviderMock.Object);

            // Act
            var sortTermList = parser.GetParsedTerms(input);

            // Assert
            sortTermList.Should().BeEmpty();
        }

        [Fact]
        public void Parser_Returns_No_SortTerm_When_Input_Is_Empty()
        {
            // Arrange
            var input = string.Empty;
            var optionsProviderMock = new Mock<IStrainerOptionsProvider>();
            optionsProviderMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var formatter = new DescendingPrefixSortingWayFormatter();
            var parser = new SortTermParser(formatter, optionsProviderMock.Object);

            // Act
            var sortTermList = parser.GetParsedTerms(input);

            // Assert
            sortTermList.Should().BeEmpty();
        }

        [Fact]
        public void Parser_Returns_NoSortTerm_When_Input_Is_OnlyWhitespace()
        {
            // Arrange
            var input = string.Empty;
            var optionsProviderMock = new Mock<IStrainerOptionsProvider>();
            optionsProviderMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var formatter = new DescendingPrefixSortingWayFormatter();
            var parser = new SortTermParser(formatter, optionsProviderMock.Object);

            // Act
            var sortTermList = parser.GetParsedTerms(input);

            // Assert
            sortTermList.Should().BeEmpty();
        }
    }
}
