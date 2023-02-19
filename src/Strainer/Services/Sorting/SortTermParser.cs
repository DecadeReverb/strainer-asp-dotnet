using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Models.Sorting.Terms;
using System.Text.RegularExpressions;

namespace Fluorite.Strainer.Services.Sorting
{
    public class SortTermParser : ISortTermParser
    {
        private const string EscapedCommaPattern = @"(?<!($|[^\\])(\\\\)*?\\),";

        private readonly ISortingWayFormatter _formatter;
        private readonly IStrainerOptionsProvider _strainerOptionsProvider;

        public SortTermParser(ISortingWayFormatter formatter, IStrainerOptionsProvider strainerOptionsProvider)
        {
            _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
            _strainerOptionsProvider = strainerOptionsProvider ?? throw new ArgumentNullException(nameof(strainerOptionsProvider));
        }

        public IList<ISortTerm> GetParsedTerms(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return new List<ISortTerm>();
            }

            input = input.Trim();
            var value = new List<ISortTerm>();
            var options = _strainerOptionsProvider.GetStrainerOptions();

            foreach (var part in Regex.Split(input, EscapedCommaPattern))
            {
                if (string.IsNullOrWhiteSpace(part))
                {
                    continue;
                }

                var sortingWay = _formatter.GetSortingWay(part);
                if (sortingWay == SortingWay.Unknown)
                {
                    sortingWay = options.DefaultSortingWay;
                }

                var sortTerm = new SortTerm()
                {
                    Input = part,
                    IsDescending = sortingWay == SortingWay.Descending,
                    Name = _formatter.Unformat(part, sortingWay),
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
