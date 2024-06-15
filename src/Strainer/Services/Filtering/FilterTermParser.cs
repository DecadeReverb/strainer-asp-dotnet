using Fluorite.Strainer.Models.Filtering.Terms;
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

    public IList<IFilterTerm> GetParsedTerms(string input)
    {
        if (string.IsNullOrWhiteSpace(input))
        {
            return new List<IFilterTerm>();
        }

        var list = new List<IFilterTerm>();
        foreach (var filter in Regex.Split(input, EscapedCommaPattern))
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                continue;
            }

            string filterTermInput = filter;

            if (filter.StartsWith("("))
            {
                var filterOperatorAndValue = ParseFilterOperatorAndValue(filter);
                var subfilters = ParseSubfilters(filter, filterOperatorAndValue);
                filterTermInput = subfilters + filterOperatorAndValue;
            }

            var filterTerm = ParseFilterTerm(filterTermInput);
            if (filterTerm is null)
            {
                continue;
            }

            list.Add(filterTerm);
        }

        return list;
    }

    private IFilterTerm ParseFilterTerm(string input)
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
            Values = values,
            Operator = operatorParsed,
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
