﻿using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Sieve.Models;

namespace Sieve.Services.Sorting
{
    public class SortTermParser : ISortTermParser
    {
        private const string EscapedCommaPattern = @"(?<!($|[^\\])(\\\\)*?\\),";

        public SortTermParser()
        {

        }

        public IList<ISortTerm> GetParsedTerms(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new List<ISortTerm>();
            }

            var value = new List<ISortTerm>();
            foreach (var part in Regex.Split(input, EscapedCommaPattern))
            {
                if (string.IsNullOrWhiteSpace(part))
                {
                    continue;
                }

                var sortTerm = new SortTerm()
                {
                    Input = part
                };

                if (!value.Any(s => s.Name == sortTerm.Name))
                {
                    value.Add(sortTerm);
                }
            }

            return value;
        }
    }
}