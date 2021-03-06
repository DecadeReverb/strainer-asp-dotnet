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

        private readonly IFilterOperatorParser _parser;
        private readonly IConfigurationFilterOperatorsProvider _filterOperatorsConfigurationProvider;

        public FilterTermParser(
            IFilterOperatorParser parser,
            IConfigurationFilterOperatorsProvider filterOperatorsConfigurationProvider)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
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

                if (!list.Any(f => f.Names.Any(n => filterTerm.Names.Any(n2 => n2 == n))))
                {
                    list.Add(filterTerm);
                }
            }

            return list;
        }

        private IFilterTerm ParseFilterTerm(string input)
        {
            var (filterName, filterOpertatorSymbol, filterValues) = GetFilterSplits(input);
            var names = GetFilterNames(filterName);

            if (!names.Any())
            {
                return null;
            }

            var values = GetFilterValues(filterValues);
            var operatorParsed = _parser.GetParsedOperator(filterOpertatorSymbol);

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

        private List<string> GetFilterNames(string names)
        {
            if (names.Equals(string.Empty))
            {
                return new List<string>();
            }

            return Regex.Split(names, EscapedPipePattern)
                .Select(filterName => filterName.Trim())
                .Where(filterName => !string.IsNullOrWhiteSpace(filterName))
                .ToList();
        }

        private List<string> GetFilterValues(string values)
        {
            if (values.Equals(string.Empty))
            {
                return new List<string>();
            }

            return Regex.Split(values, EscapedPipePattern)
                .Select(t => t.Trim())
                .ToList();
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
