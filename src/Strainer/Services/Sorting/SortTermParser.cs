using Fluorite.Strainer.Models.Sorting.Terms;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Fluorite.Strainer.Services.Sorting
{
    public class SortTermParser : ISortTermParser
    {
        private const string EscapedCommaPattern = @"(?<!($|[^\\])(\\\\)*?\\),";

        private readonly ISortingWayFormatter _formatter;

        public SortTermParser(ISortingWayFormatter formatter)
        {
            _formatter = formatter ?? throw new System.ArgumentNullException(nameof(formatter));
        }

        public IList<ISortTerm> GetParsedTerms(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new List<ISortTerm>();
            }

            input = input.Trim();
            var value = new List<ISortTerm>();

            foreach (var part in Regex.Split(input, EscapedCommaPattern))
            {
                if (string.IsNullOrWhiteSpace(part))
                {
                    continue;
                }

                var sortTerm = new SortTerm()
                {
                    Input = part,
                    IsDescending = _formatter.IsDescending(part),
                    Name = _formatter.Unformat(part),
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
