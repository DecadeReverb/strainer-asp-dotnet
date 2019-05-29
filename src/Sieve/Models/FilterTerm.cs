using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sieve.Models.Filtering.Operators;

namespace Sieve.Models
{
    public class FilterTerm : IFilterTerm, IEquatable<FilterTerm>
    {
        private const string EscapedPipePattern = @"(?<!($|[^\\])(\\\\)*?\\)\|";

        // TODO:
        // Move this to public filter operators options or some kind of
        // filter operators provider.
        // A DTO should not have hardcoded magic strings.
        private static readonly string[] Operators = new string[] {
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

        private readonly IList<IFilterOperator> _operators;

        public FilterTerm()
        {
            // TODO:
            // Change this to external provider, or move this logic it at all
            // to filter operator provider. DTO should not handle creating
            // the operators, it should just have it ready from the start.
            _operators = (typeof(FilterTerm))
                .Assembly
                .DefinedTypes
                .Where(t =>
                    t.ImplementedInterfaces.Contains(typeof(IFilterOperator))
                    && !t.IsAbstract)
                .Select(t => Activator.CreateInstance(t))
                .OfType<IFilterOperator>()
                .ToList();
        }

        // TODO:
        // Move this to some kind of filter builder service.
        // A DTO should not have setter this complex.
        public string Filter
        {
            set
            {
                var filterSplits = value.Split(Operators, StringSplitOptions.RemoveEmptyEntries)
                    .Select(t => t.Trim()).ToArray();
                Names = Regex.Split(filterSplits[0], EscapedPipePattern).Select(t => t.Trim()).ToArray();
                Values = filterSplits.Length > 1 ? Regex.Split(filterSplits[1], EscapedPipePattern).Select(t => t.Trim()).ToArray() : null;
                Operator = Array.Find(Operators, o => value.Contains(o)) ?? "==";
                OperatorParsed = GetOperatorParsed(Operator);
                OperatorIsCaseInsensitive = Operator.EndsWith("*");
                OperatorIsNegated = !(OperatorParsed is NotEqualsOperator) && Operator.StartsWith("!");
            }

        }

        public string[] Names { get; private set; }

        public string Operator { get; private set; }

        public IFilterOperator OperatorParsed { get; private set; }

        public string[] Values { get; private set; }

        public bool OperatorIsCaseInsensitive { get; private set; }

        public bool OperatorIsNegated { get; private set; }

        public bool Equals(FilterTerm other)
        {
            return Names.SequenceEqual(other.Names)
                && Values.SequenceEqual(other.Values)
                && Operator == other.Operator;
        }

        // TODO:
        // Move this to some kind of operator parser service.
        // A DTO should not take care of string parsing.
        //
        // To consider if this SHOULD return null if no operator was found.
        // Fallbacking to default filter operator should be handled by some
        // service, not DTO or simple parsing method. Parsing method should
        // only parse input, nothing more.
        private IFilterOperator GetOperatorParsed(string @operator)
        {
            // TODO:
            // Store somewhere info about case insensitivity asterisk suffix
            // and negation exclamation mark prefix.
            return _operators.FirstOrDefault(f =>
            {
                return f.Operator == @operator.TrimEnd('*')     // Case sensivity variations
                    || f.Operator == @operator.TrimStart('!');  // Negated variations
            }) ?? new EqualsOperator();
        }
    }
}
