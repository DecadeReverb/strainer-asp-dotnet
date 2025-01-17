﻿using Fluorite.Strainer.Models.Configuration;
using Fluorite.Strainer.Services.Configuration;
using Fluorite.Strainer.Services.Filtering;

namespace Fluorite.Strainer.UnitTests.Services.Filtering;

public class FilterTermParserTests
{
    [Fact]
    public void Parser_ReturnsNoFilterTerms_When_InputIsNull()
    {
        // Arrange
        string input = null;
        var filterOperators = FilterOperatorMapper.DefaultOperators;
        var strainerConfigurationMock = Substitute.For<IStrainerConfiguration>();
        strainerConfigurationMock
            .FilterOperators
            .Returns(filterOperators);
        var strainerConfigurationProvider = new StrainerConfigurationProvider(strainerConfigurationMock);
        var filterOperatorsProvider = new ConfigurationFilterOperatorsProvider(strainerConfigurationProvider);
        var operatorParser = new FilterOperatorParser(filterOperatorsProvider);
        var namesParser = new FilterTermNamesParser();
        var valuesParser = new FilterTermValuesParser();
        var sectionsParser = new FilterTermSectionsParser(filterOperatorsProvider);
        var termParser = new FilterTermParser(operatorParser, namesParser, valuesParser, sectionsParser);

        // Act
        var filterTermList = termParser.GetParsedTerms(input);

        // Assert
        filterTermList.Should().BeEmpty();
    }

    [Fact]
    public void Parser_ReturnsNoFilterTerms_When_InputIsEmpty()
    {
        // Arrange
        var input = string.Empty;
        var filterOperators = FilterOperatorMapper.DefaultOperators;
        var strainerConfigurationMock = Substitute.For<IStrainerConfiguration>();
        strainerConfigurationMock
            .FilterOperators
            .Returns(filterOperators);
        var strainerConfigurationProvider = new StrainerConfigurationProvider(strainerConfigurationMock);
        var filterOperatorsProvider = new ConfigurationFilterOperatorsProvider(strainerConfigurationProvider);
        var operatorParser = new FilterOperatorParser(filterOperatorsProvider);
        var namesParser = new FilterTermNamesParser();
        var valuesParser = new FilterTermValuesParser();
        var sectionsParser = new FilterTermSectionsParser(filterOperatorsProvider);
        var termParser = new FilterTermParser(operatorParser, namesParser, valuesParser, sectionsParser);

        // Act
        var filterTermList = termParser.GetParsedTerms(input);

        // Assert
        filterTermList.Should().BeEmpty();
    }

    [Fact]
    public void Parser_ReturnsNoFilterTerms_When_InputIsOnlyWhitespace()
    {
        // Arrange
        var input = " ";
        var filterOperators = FilterOperatorMapper.DefaultOperators;
        var strainerConfigurationMock = Substitute.For<IStrainerConfiguration>();
        strainerConfigurationMock
            .FilterOperators
            .Returns(filterOperators);
        var strainerConfigurationProvider = new StrainerConfigurationProvider(strainerConfigurationMock);
        var filterOperatorsProvider = new ConfigurationFilterOperatorsProvider(strainerConfigurationProvider);
        var operatorParser = new FilterOperatorParser(filterOperatorsProvider);
        var namesParser = new FilterTermNamesParser();
        var valuesParser = new FilterTermValuesParser();
        var sectionsParser = new FilterTermSectionsParser(filterOperatorsProvider);
        var termParser = new FilterTermParser(operatorParser, namesParser, valuesParser, sectionsParser);

        // Act
        var filterTermList = termParser.GetParsedTerms(input);

        // Assert
        filterTermList.Should().BeEmpty();
    }

    [Fact]
    public void Parser_ReturnsNoFilterTerms_When_ThereIsNoName()
    {
        // Arrange
        var input = "==SomeValue";
        var filterOperators = FilterOperatorMapper.DefaultOperators;
        var strainerConfigurationMock = Substitute.For<IStrainerConfiguration>();
        strainerConfigurationMock
            .FilterOperators
            .Returns(filterOperators);
        var strainerConfigurationProvider = new StrainerConfigurationProvider(strainerConfigurationMock);
        var filterOperatorsProvider = new ConfigurationFilterOperatorsProvider(strainerConfigurationProvider);
        var operatorParser = new FilterOperatorParser(filterOperatorsProvider);
        var namesParser = new FilterTermNamesParser();
        var valuesParser = new FilterTermValuesParser();
        var sectionsParser = new FilterTermSectionsParser(filterOperatorsProvider);
        var termParser = new FilterTermParser(operatorParser, namesParser, valuesParser, sectionsParser);

        // Act
        var filterTermList = termParser.GetParsedTerms(input);

        // Assert
        filterTermList.Should().BeEmpty();
    }

    [Fact]
    public void Parser_ReturnsNoFilterTerms_When_ThereIsOnlyOperator()
    {
        // Arrange
        var input = "==";
        var filterOperators = FilterOperatorMapper.DefaultOperators;
        var strainerConfigurationMock = Substitute.For<IStrainerConfiguration>();
        strainerConfigurationMock
            .FilterOperators
            .Returns(filterOperators);
        var strainerConfigurationProvider = new StrainerConfigurationProvider(strainerConfigurationMock);
        var filterOperatorsProvider = new ConfigurationFilterOperatorsProvider(strainerConfigurationProvider);
        var operatorParser = new FilterOperatorParser(filterOperatorsProvider);
        var namesParser = new FilterTermNamesParser();
        var valuesParser = new FilterTermValuesParser();
        var sectionsParser = new FilterTermSectionsParser(filterOperatorsProvider);
        var termParser = new FilterTermParser(operatorParser, namesParser, valuesParser, sectionsParser);

        // Act
        var filterTermList = termParser.GetParsedTerms(input);

        // Assert
        filterTermList.Should().BeEmpty();
    }

    [Fact]
    public void Parser_ReturnsFilterTerm_When_ThereIsOnlyName()
    {
        // Arrange
        var input = "CustomFilterName";
        var filterOperators = FilterOperatorMapper.DefaultOperators;
        var strainerConfigurationMock = Substitute.For<IStrainerConfiguration>();
        strainerConfigurationMock
            .FilterOperators
            .Returns(filterOperators);
        var strainerConfigurationProvider = new StrainerConfigurationProvider(strainerConfigurationMock);
        var filterOperatorsProvider = new ConfigurationFilterOperatorsProvider(strainerConfigurationProvider);
        var operatorParser = new FilterOperatorParser(filterOperatorsProvider);
        var namesParser = new FilterTermNamesParser();
        var valuesParser = new FilterTermValuesParser();
        var sectionsParser = new FilterTermSectionsParser(filterOperatorsProvider);
        var termParser = new FilterTermParser(operatorParser, namesParser, valuesParser, sectionsParser);

        // Act
        var filterTermList = termParser.GetParsedTerms(input);

        // Assert
        filterTermList.Should().HaveCount(1);
    }

    [Fact]
    public void Parser_ReturnsFilterTerm_When_ThereIsNameWithOperator()
    {
        // Arrange
        var input = "CustomFilterName==";
        var filterOperators = FilterOperatorMapper.DefaultOperators;
        var strainerConfigurationMock = Substitute.For<IStrainerConfiguration>();
        strainerConfigurationMock
            .FilterOperators
            .Returns(filterOperators);
        var strainerConfigurationProvider = new StrainerConfigurationProvider(strainerConfigurationMock);
        var filterOperatorsProvider = new ConfigurationFilterOperatorsProvider(strainerConfigurationProvider);
        var operatorParser = new FilterOperatorParser(filterOperatorsProvider);
        var namesParser = new FilterTermNamesParser();
        var valuesParser = new FilterTermValuesParser();
        var sectionsParser = new FilterTermSectionsParser(filterOperatorsProvider);
        var termParser = new FilterTermParser(operatorParser, namesParser, valuesParser, sectionsParser);

        // Act
        var filterTermList = termParser.GetParsedTerms(input);

        // Assert
        filterTermList.Should().HaveCount(1);
    }
}
