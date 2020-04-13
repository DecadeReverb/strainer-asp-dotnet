using FluentAssertions;
using Fluorite.Strainer.Models.Configuration;
using Fluorite.Strainer.Services.Configuration;
using Fluorite.Strainer.Services.Filtering;
using Moq;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Filtering
{
    public class FilterTermParserTests
    {
        [Fact]
        public void Parser_ReturnsNoFilterTerms_WhenInputIsNull()
        {
            // Arrange
            string input = null;
            var filterOperators = FilterOperatorMapper.DefaultOperators
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            var strainerConfigurationMock = new Mock<IStrainerConfiguration>();
            strainerConfigurationMock.SetupGet(configuration => configuration.FilterOperators)
                .Returns(filterOperators);
            var strainerConfigurationProvider = new StrainerConfigurationProvider(strainerConfigurationMock.Object);
            var filterOperatorsProvider = new ConfigurationFilterOperatorsProvider(strainerConfigurationProvider);
            var operatorParser = new FilterOperatorParser(filterOperatorsProvider);
            var termParser = new FilterTermParser(operatorParser, filterOperatorsProvider);

            // Act
            var filterTermList = termParser.GetParsedTerms(input);

            // Assert
            filterTermList.Should().BeEmpty();
        }

        [Fact]
        public void Parser_ReturnsNoFilterTerms_WhenInputIsEmpty()
        {
            // Arrange
            var input = string.Empty;
            var filterOperators = FilterOperatorMapper.DefaultOperators
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            var strainerConfigurationMock = new Mock<IStrainerConfiguration>();
            strainerConfigurationMock.SetupGet(configuration => configuration.FilterOperators)
                .Returns(filterOperators);
            var strainerConfigurationProvider = new StrainerConfigurationProvider(strainerConfigurationMock.Object);
            var filterOperatorsProvider = new ConfigurationFilterOperatorsProvider(strainerConfigurationProvider);
            var operatorParser = new FilterOperatorParser(filterOperatorsProvider);
            var termParser = new FilterTermParser(operatorParser, filterOperatorsProvider);

            // Act
            var filterTermList = termParser.GetParsedTerms(input);

            // Assert
            filterTermList.Should().BeEmpty();
        }

        [Fact]
        public void Parser_ReturnsNoFilterTerms_WhenInputIsOnlyWhitespace()
        {
            // Arrange
            var input = string.Empty;
            var filterOperators = FilterOperatorMapper.DefaultOperators
                .ToDictionary(pair => pair.Key, pair => pair.Value);
            var strainerConfigurationMock = new Mock<IStrainerConfiguration>();
            strainerConfigurationMock.SetupGet(configuration => configuration.FilterOperators)
                .Returns(filterOperators);
            var strainerConfigurationProvider = new StrainerConfigurationProvider(strainerConfigurationMock.Object);
            var filterOperatorsProvider = new ConfigurationFilterOperatorsProvider(strainerConfigurationProvider);
            var operatorParser = new FilterOperatorParser(filterOperatorsProvider);
            var termParser = new FilterTermParser(operatorParser, filterOperatorsProvider);

            // Act
            var filterTermList = termParser.GetParsedTerms(input);

            // Assert
            filterTermList.Should().BeEmpty();
        }
    }
}
