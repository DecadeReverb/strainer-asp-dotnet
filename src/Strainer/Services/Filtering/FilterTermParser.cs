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

        public FilterTermParser(IFilterOperatorParser parser, IFilterOperatorMapper mapper)
        {
            Mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            Parser = parser ?? throw new ArgumentNullException(nameof(parser));
        }

        protected IFilterOperatorMapper Mapper { get; }

        protected IFilterOperatorParser Parser { get; }

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
                    if (!list.Any(f => f.Names.Any(n => filterTerm.Names.Any(n2 => n2 == n))))
                    {
                        list.Add(filterTerm);
                    }
                }
                else
                {
                    var filterTerm = ParseFilterTerm(filter);
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
            return new string(input.Except(string.Concat(filterSplits).ToArray()).ToArray());
        }

        private List<string> GetFilterSplits(string input)
        {
            var symbols = Mapper
                .Symbols
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
            var operatorParsed = Parser.GetParsedOperator(symbol);

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
