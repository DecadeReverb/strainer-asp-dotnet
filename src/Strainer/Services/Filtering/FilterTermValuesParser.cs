using System.Text.RegularExpressions;

namespace Fluorite.Strainer.Services.Filtering
{
    public class FilterTermValuesParser : IFilterTermValuesParser
    {
        private const string EscapedPipePattern = @"(?<!($|[^\\])(\\\\)*?\\)\|";

        public IList<string> Parse(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return new List<string>();
            }

            return Regex.Split(input, EscapedPipePattern)
                .Select(t => t.Trim())
                .ToList();
        }
    }
}
