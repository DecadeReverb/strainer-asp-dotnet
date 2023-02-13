using System.Text.RegularExpressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterTermNamesParser : IFilterTermNamesParser
    {
        private const string EscapedPipePattern = @"(?<!($|[^\\])(\\\\)*?\\)\|";

        public IList<string> Parse(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new List<string>();
            }

            return Regex.Split(input, EscapedPipePattern)
                .Select(filterName => filterName.Trim())
                .Where(filterName => !string.IsNullOrWhiteSpace(filterName))
                .ToList();
        }
    }
}
