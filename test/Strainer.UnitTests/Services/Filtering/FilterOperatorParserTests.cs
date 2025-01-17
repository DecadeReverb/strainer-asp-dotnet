﻿using Fluorite.Strainer.Models.Configuration;
using Fluorite.Strainer.Services.Configuration;
using Fluorite.Strainer.Services.Filtering;

namespace Fluorite.Strainer.UnitTests.Services.Filtering;

public class FilterOperatorParserTests
{
    [Fact]
    public void Parser_ReturnsNullFilterOperator_WhenInputIsNull()
    {
        // Arrange
        string symbol = null;
        var filterOperators = FilterOperatorMapper.DefaultOperators;
        var strainerConfigurationMock = Substitute.For<IStrainerConfiguration>();
        strainerConfigurationMock
            .FilterOperators
            .Returns(filterOperators);
        var strainerConfigurationProvider = new StrainerConfigurationProvider(strainerConfigurationMock);
        var filterOperatorsProvider = new ConfigurationFilterOperatorsProvider(strainerConfigurationProvider);
        var parser = new FilterOperatorParser(filterOperatorsProvider);

        // Act
        var filterOperator = parser.GetParsedOperator(symbol);

        // Assert
        filterOperator.Should().BeNull();
    }

    [Fact]
    public void Parser_ReturnsDefaultFilterOperator_WhenInputIsEmpty()
    {
        // Arrange
        var symbol = string.Empty;
        var filterOperators = FilterOperatorMapper.DefaultOperators;
        var strainerConfigurationMock = Substitute.For<IStrainerConfiguration>();
        strainerConfigurationMock
            .FilterOperators
            .Returns(filterOperators);
        var strainerConfigurationProvider = new StrainerConfigurationProvider(strainerConfigurationMock);
        var filterOperatorsProvider = new ConfigurationFilterOperatorsProvider(strainerConfigurationProvider);
        var parser = new FilterOperatorParser(filterOperatorsProvider);

        // Act
        var filterOperator = parser.GetParsedOperator(symbol);

        // Assert
        filterOperator.Should().BeNull();
    }

    [Fact]
    public void Parser_ReturnsDefaultFilterOperator_WhenInputIsWhitespace()
    {
        // Arrange
        var symbol = " ";
        var filterOperators = FilterOperatorMapper.DefaultOperators;
        var strainerConfigurationMock = Substitute.For<IStrainerConfiguration>();
        strainerConfigurationMock
            .FilterOperators
            .Returns(filterOperators);
        var strainerConfigurationProvider = new StrainerConfigurationProvider(strainerConfigurationMock);
        var filterOperatorsProvider = new ConfigurationFilterOperatorsProvider(strainerConfigurationProvider);
        var parser = new FilterOperatorParser(filterOperatorsProvider);

        // Act
        var filterOperator = parser.GetParsedOperator(symbol);

        // Assert
        filterOperator.Should().BeNull();
    }

    [Fact]
    public void Parser_ReturnsOperators_For_AllFilterOperator_InMapper()
    {
        // Arrange
        var filterOperators = FilterOperatorMapper.DefaultOperators;
        var strainerConfigurationMock = Substitute.For<IStrainerConfiguration>();
        strainerConfigurationMock
            .FilterOperators
            .Returns(filterOperators);
        var strainerConfigurationProvider = new StrainerConfigurationProvider(strainerConfigurationMock);
        var filterOperatorsProvider = new ConfigurationFilterOperatorsProvider(strainerConfigurationProvider);
        var parser = new FilterOperatorParser(filterOperatorsProvider);

        // Act
        var result = filterOperators.Keys.Select(symbol => parser.GetParsedOperator(symbol));

        // Assert
        result.Should().BeEquivalentTo(filterOperators.Values);
    }
}
