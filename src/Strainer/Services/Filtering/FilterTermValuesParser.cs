namespace Fluorite.Strainer.Services.Filtering;

public class FilterTermValuesParser : IFilterTermValuesParser
{
    public IList<string> Parse(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return new List<string>();
        }

        return input.Split(new[] { '|' }, StringSplitOptions.None)
            .Select(t => t.Trim().Replace(@"\,", ","))
            .ToList();
    }
}
