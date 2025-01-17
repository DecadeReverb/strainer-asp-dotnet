﻿using Fluorite.Strainer.Models.Filtering.Terms;
using System.Text.RegularExpressions;

namespace Fluorite.Strainer.Services.Filtering;

public class FilterTermParser : IFilterTermParser
{
    private const string EscapedCommaPattern = @"(?<!($|[^\\])(\\\\)*?\\),";
    private const string EscapedPipePattern = @"(?<!($|[^\\])(\\\\)*?\\)\|";

    private readonly IFilterOperatorParser _operatorParser;
    private readonly IFilterTermNamesParser _namesParser;
    private readonly IFilterTermValuesParser _valuesParser;
    private readonly IFilterTermSectionsParser _termSectionsParser;

    public FilterTermParser(
        IFilterOperatorParser operatorParser,
        IFilterTermNamesParser namesParser,
        IFilterTermValuesParser valuesParser,
        IFilterTermSectionsParser termSectionsParser)
    {
        _operatorParser = Guard.Against.Null(operatorParser);
        _namesParser = Guard.Against.Null(namesParser);
        _valuesParser = Guard.Against.Null(valuesParser);
        _termSectionsParser = Guard.Against.Null(termSectionsParser);
    }

    public IList<IFilterTerm> GetParsedTerms(string? input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new List<IFilterTerm>();
        }

        var terms = new List<IFilterTerm>();
        var inputParts = Regex.Split(input, EscapedCommaPattern, RegexOptions.None, TimeSpan.FromMilliseconds(100));

        foreach (var filterPart in inputParts)
        {
            if (string.IsNullOrWhiteSpace(filterPart))
            {
                continue;
            }

            string filterTermInput = filterPart;

            if (filterPart.StartsWith("("))
            {
                var filterOperatorAndValue = ParseFilterOperatorAndValue(filterPart);
                var subfilters = ParseSubfilters(filterPart, filterOperatorAndValue);
                filterTermInput = subfilters + filterOperatorAndValue;
            }

            var filterTerm = ParseFilterTerm(filterTermInput);
            if (filterTerm is null)
            {
                continue;
            }

            terms.Add(filterTerm);
        }

        return terms;
    }

    private IFilterTerm? ParseFilterTerm(string input)
    {
        var sections = _termSectionsParser.Parse(input);
        var names = _namesParser.Parse(sections.Names);

        if (!names.Any())
        {
            return null;
        }

        var values = _valuesParser.Parse(sections.Values);
        var operatorParsed = _operatorParser.GetParsedOperator(sections.OperatorSymbol);

        return new FilterTerm(input)
        {
            Names = names,
            Operator = operatorParsed,
            Values = values,
        };
    }

    private string ParseFilterOperatorAndValue(string filter)
    {
        return filter.Substring(filter.LastIndexOf(")") + 1);
    }

    private string ParseSubfilters(string filter, string filterOpAndVal)
    {
        return filter
            .Replace(filterOpAndVal, string.Empty)
            .Replace("(", string.Empty)
            .Replace(")", string.Empty);
    }
}
