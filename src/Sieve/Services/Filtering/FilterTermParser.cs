using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Strainer.Models;
using Strainer.Models.Filtering.Operators;

namespace Strainer.Services.Filtering
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
                    var filterOpAndVal = filter.Substring(filter.LastIndexOf(")") + 1);
                    var subfilters = filter
                        .Replace(filterOpAndVal, "")
                        .Replace("(", "")
                        .Replace(")", "");

                    var filterTerm = ParseFilterTerm(subfilters + filterOpAndVal);

                    if (!list.Any(f => f.Names.Any(n => filterTerm.Names.Any(n2 => n2 == n))))
                    {
                        list.Add(filterTerm);
                    }
                }
                else
                {
                    var filterTerm = ParseFilterTerm(filter);
                    list.Add(filterTerm);
                }
            }

            return list;
        }

        private IFilterTerm ParseFilterTerm(string input)
        {
            var filterSplits = input
                .Split(AllOperators, StringSplitOptions.RemoveEmptyEntries)
                .Select(t => t.Trim())
                .ToList();
            var names = Regex.Split(filterSplits.First(), EscapedPipePattern)
                .Select(t => t.Trim())
                .ToList();
            var values = filterSplits.Count() > 1
                ? Regex.Split(filterSplits[1], EscapedPipePattern)
                    .Select(t => t.Trim())
                    .ToList()
                : null;
            var @operator = Array.Find(AllOperators, o => input.Contains(o))
                ?? "==";
            var operatorParsed = Parser.GetParsedOperatorAsUnnegated(@operator);
            var operatorIsCaseInsensitive = @operator.EndsWith("*");
            var operatorIsNegated = !(operatorParsed is NotEqualsOperator) && @operator.StartsWith("!");

            return new FilterTerm
            {
                Input = input,
                Names = names,
                Values = values,
                Operator = @operator,
                OperatorIsCaseInsensitive = operatorIsCaseInsensitive,
                OperatorIsNegated = operatorIsNegated,
                OperatorParsed = operatorParsed,
            };
        }
    }
}
