using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Models.Sorting.Terms;

namespace Fluorite.Strainer.Services.Sorting;

public class SortTermParser : ISortTermParser
{
    private readonly ISortingWayFormatter _formatter;
    private readonly IStrainerOptionsProvider _strainerOptionsProvider;
    private readonly ISortTermValueParser _sortTermValueParser;

    public SortTermParser(
        ISortingWayFormatter formatter,
        IStrainerOptionsProvider strainerOptionsProvider,
        ISortTermValueParser sortTermValueParser)
    {
        _formatter = formatter ?? throw new ArgumentNullException(nameof(formatter));
        _strainerOptionsProvider = strainerOptionsProvider ?? throw new ArgumentNullException(nameof(strainerOptionsProvider));
        _sortTermValueParser = sortTermValueParser ?? throw new ArgumentNullException(nameof(sortTermValueParser));
    }

    public IList<ISortTerm> GetParsedTerms(string input)
    {
        if (string.IsNullOrEmpty(input))
        {
            return new List<ISortTerm>();
        }

        var values = _sortTermValueParser.GetParsedValues(input);
        if (!values.Any())
        {
            return new List<ISortTerm>();
        }

        var terms = new List<ISortTerm>();
        var options = _strainerOptionsProvider.GetStrainerOptions();

        foreach (var value in values)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                continue;
            }

            var sortingWay = _formatter.GetSortingWay(value);
            if (sortingWay == SortingWay.Unknown)
            {
                sortingWay = options.DefaultSortingWay;
            }

            var sortTerm = new SortTerm()
            {
                Input = value,
                IsDescending = sortingWay == SortingWay.Descending,
                Name = _formatter.Unformat(value, sortingWay),
            };

            if (!terms.Any(s => s.Name == sortTerm.Name))
            {
                terms.Add(sortTerm);
            }
        }

        return terms;
    }
}
