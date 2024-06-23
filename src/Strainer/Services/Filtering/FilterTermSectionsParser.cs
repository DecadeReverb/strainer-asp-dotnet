using Fluorite.Strainer.Models.Filtering.Terms;
using System.Text.RegularExpressions;

namespace Fluorite.Strainer.Services.Filtering;

public class FilterTermSectionsParser : IFilterTermSectionsParser
{
    private readonly IConfigurationFilterOperatorsProvider _filterOperatorsConfigurationProvider;

    public FilterTermSectionsParser(
        IConfigurationFilterOperatorsProvider filterOperatorsConfigurationProvider)
    {
        _filterOperatorsConfigurationProvider = Guard.Against.Null(filterOperatorsConfigurationProvider);
    }

    public FilterTermSections Parse(string input)
    {
        Guard.Against.Null(input);

        var symbols = _filterOperatorsConfigurationProvider
            .GetFilterOperators()
            .Keys
            .OrderByDescending(s => s.Length)
            .ToArray();
        var splitPattern = string.Join("|", symbols.Select(s => $"({Regex.Escape(s)})"));
        var substrings = Regex.Split(input, splitPattern, RegexOptions.None, TimeSpan.FromMilliseconds(100));

        if (substrings.Length <= 1)
        {
            return new FilterTermSections
            {
                Names = substrings.FirstOrDefault(),
                OperatorSymbol = string.Empty,
                Values = string.Empty,
            };
        }

        if (substrings.Length >= 3)
        {
            return new FilterTermSections
            {
                Names = substrings[0],
                OperatorSymbol = substrings[1],
                Values = substrings[2],
            };
        }

        return new FilterTermSections
        {
            Names = string.Empty,
            OperatorSymbol = string.Empty,
            Values = string.Empty,
        };
    }
}
