using Fluorite.Strainer.Models.Filtering.Terms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterTermParser : IFilterTermParser
    {
        private const string EscapedCommaPattern = @"(?<!($|[^\\])(\\\\)*?\\),";
        private const string EscapedPipePattern = @"(?<!($|[^\\])(\\\\)*?\\)\|";

        private readonly IFilterOperatorParser _operatorParser;
        private readonly IFilterTermNamesParser _namesParser;
        private readonly IFilterTermValuesParser _valuesParser;
        private readonly IConfigurationFilterOperatorsProvider _filterOperatorsConfigurationProvider;

        public FilterTermParser(
            IFilterOperatorParser operatorParser,
            IFilterTermNamesParser namesParser,
            IFilterTermValuesParser valuesParser,
            IConfigurationFilterOperatorsProvider filterOperatorsConfigurationProvider)
        {
            _operatorParser = operatorParser ?? throw new ArgumentNullException(nameof(_operatorParser));
            _namesParser = namesParser ?? throw new ArgumentNullException(nameof(namesParser));
            _valuesParser = valuesParser ?? throw new ArgumentNullException(nameof(valuesParser));
            _filterOperatorsConfigurationProvider = filterOperatorsConfigurationProvider
                ?? throw new ArgumentNullException(nameof(filterOperatorsConfigurationProvider));
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
            var (filterName, filterOpertatorSymbol, filterValues) = GetFilterSplits(input);
            var names = _namesParser.Parse(filterName);

            if (!names.Any())
            {
                return null;
            }

            var values = _valuesParser.Parse(filterValues);
            var operatorParsed = _operatorParser.GetParsedOperator(filterOpertatorSymbol);

            return new FilterTerm(input)
            {
                Names = names,
                Values = values,
                Operator = operatorParsed,
            };
        }

        private (string FilterName, string FilterOperatorSymbol, string FilterValues) GetFilterSplits(string input)
        {
            var symbols = _filterOperatorsConfigurationProvider
                .GetFilterOperators()
                .Keys
                .OrderByDescending(s => s.Length)
                .ToArray();
            var splitPattern = string.Join("|", symbols.Select(s => $"({Regex.Escape(s)})"));
            var substrings = Regex.Split(input, splitPattern);

            if (substrings.Length <= 1)
            {
                return (substrings.FirstOrDefault(), string.Empty, string.Empty);
            }

            if (substrings.Length >= 3)
            {
                return (substrings[0], substrings[1], substrings[2]);
            }

            return (string.Empty, string.Empty, string.Empty);
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
}
