using Fluorite.Strainer.Models.Filtering.Operators;
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
        private readonly IReadOnlyDictionary<string, IFilterOperator> _filterOperators;

        public FilterTermParser(IFilterOperatorParser parser, IReadOnlyDictionary<string, IFilterOperator> filterOperators)
        {
            _parser = parser ?? throw new ArgumentNullException(nameof(parser));
            this._filterOperators = filterOperators ?? throw new ArgumentNullException(nameof(filterOperators));
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

                if (filter.StartsWith("("))
                {
                    var filterOperatorAndValue = ParseFilterOperatorAndValue(filter);
                    var subfilters = ParseSubfilters(filter, filterOperatorAndValue);
                    var filterTerm = ParseFilterTerm(subfilters + filterOperatorAndValue);
                    if (filterTerm == null)
                    {
                        continue;
                    }

                    if (!list.Any(f => f.Names.Any(n => filterTerm.Names.Any(n2 => n2 == n))))
                    {
                        list.Add(filterTerm);
                    }
                }
                else
                {
                    var filterTerm = ParseFilterTerm(filter);
                    if (filterTerm == null)
                    {
                        continue;
                    }

                    if (!list.Any(f => f.Names.Any(n => filterTerm.Names.Any(n2 => n2 == n))))
                    {
                        list.Add(filterTerm);
                    }
                }
            }

            return list;
        }

        private List<string> GetFilterNames(List<string> filterSplits)
        {
            return Regex.Split(filterSplits.First(), EscapedPipePattern)
                .Select(t => t.Trim())
                .ToList();
        }

        private string GetFilterOperatorSymbol(string input, List<string> filterSplits)
        {
            foreach (var part in filterSplits)
            {
                input = input.Replace(part, string.Empty);
            }

            return input;
        }

        private List<string> GetFilterSplits(string input)
        {
            var symbols = _filterOperators
                .Keys
                .OrderByDescending(s => s.Length)
                .ToArray();

            return input
                .Split(symbols, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .ToList();
        }

        private List<string> GetFilterValues(List<string> filterSplits)
        {
            return filterSplits.Count() > 1
                ? Regex.Split(filterSplits[1], EscapedPipePattern)
                    .Select(t => t.Trim())
                    .ToList()
                : new List<string>();
        }

        private string ParseFilterOperatorAndValue(string filter)
        {
            return filter.Substring(filter.LastIndexOf(")") + 1);
        }

        private IFilterTerm ParseFilterTerm(string input)
        {
            var filterSplits = GetFilterSplits(input);
            var names = GetFilterNames(filterSplits);
            var values = GetFilterValues(filterSplits);
            var symbol = GetFilterOperatorSymbol(input, filterSplits);
            var operatorParsed = _parser.GetParsedOperator(symbol);

            if (!names.Any())
            {
                return null;
            }

            return new FilterTerm(input)
            {
                Names = names,
                Values = values,
                Operator = operatorParsed,
            };
        }

        private string ParseSubfilters(string filter, string filterOpAndVal)
        {
            return filter
                .Replace(filterOpAndVal, "")
                .Replace("(", "")
                .Replace(")", "");
        }
    }
}
