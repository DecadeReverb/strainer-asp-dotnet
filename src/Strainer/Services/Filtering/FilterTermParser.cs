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

        private static readonly string[] AllOperators = new string[] {
                    "!@=*",
                    "!_=*",
                    "!@=",
                    "!_=",
                    "==*",
                    "@=*",
                    "_=*",
                    "==",
                    "!=",
                    ">=",
                    "<=",
                    ">",
                    "<",
                    "@=",
                    "_="
        };

        public FilterTermParser(IFilterOperatorParser parser)
        {
            Parser = parser;
        }

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

        private static List<string> GetFilterNames(List<string> filterSplits)
        {
            return Regex.Split(filterSplits.First(), EscapedPipePattern)
                .Select(t => t.Trim())
                .ToList();
        }

        private static string GetFilterOperator(string input)
        {
            return Array.Find(AllOperators, o => input.Contains(o))
                ?? "==";
        }

        private static List<string> GetFilterSplits(string input)
        {
            return input
                .Split(AllOperators, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .ToList();
        }

        private static List<string> GetFilterValues(List<string> filterSplits)
        {
            return filterSplits.Count() > 1
                ? Regex.Split(filterSplits[1], EscapedPipePattern)
                    .Select(t => t.Trim())
                    .ToList()
                : null;
        }

        private static string ParseFilterOperatorAndValue(string filter)
        {
            return filter.Substring(filter.LastIndexOf(")") + 1);
        }

        private IFilterTerm ParseFilterTerm(string input)
        {
            var filterSplits = GetFilterSplits(input);
            var names = GetFilterNames(filterSplits);
            var values = GetFilterValues(filterSplits);
            var @operator = GetFilterOperator(input);
            var operatorParsed = Parser.GetParsedOperator(@operator);

            return new FilterTerm(input)
            {
                Names = names,
                Values = values,
                Operator = operatorParsed,
            };
        }

        private static string ParseSubfilters(string filter, string filterOpAndVal)
        {
            return filter
                .Replace(filterOpAndVal, "")
                .Replace("(", "")
                .Replace(")", "");
        }
    }
}
