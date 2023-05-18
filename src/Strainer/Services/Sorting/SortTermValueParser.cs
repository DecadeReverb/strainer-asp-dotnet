namespace Fluorite.Strainer.Services.Sorting
{
    public class SortTermValueParser : ISortTermValueParser
    {
        public string[] GetParsedValues(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return Array.Empty<string>();
            }

            return input
                .Trim()
                .Split(new[] { ',' }, StringSplitOptions.None)
                .ToArray();
        }
    }
}
