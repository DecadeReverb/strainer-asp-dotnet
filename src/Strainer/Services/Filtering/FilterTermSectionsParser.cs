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

        // Order by longest operator to ensure we first try to match the longest ones
        // as shorter operators are contained within longer ones and would be matched
        // first, but only partially, leaving out the rest of operator.
        var symbols = _filterOperatorsConfigurationProvider
            .GetFilterOperators()
            .Keys
            .OrderByDescending(s => s.Length)
            .ToArray();
        if (!symbols.Any())
        {
            throw new InvalidOperationException("No filter operators found.");
        }

        var splitPattern = string.Join("|", symbols.Select(s => $"({Regex.Escape(s)})"));
        var substrings = Regex.Split(input, splitPattern, RegexOptions.None, TimeSpan.FromMilliseconds(100));

        return substrings.Length switch
        {
            <= 2 => new FilterTermSections
            {
                Names = substrings.FirstOrDefault(),
                OperatorSymbol = string.Empty,
                Values = string.Empty,
            },
            >= 3 => new FilterTermSections
            {
                Names = substrings[0],
                OperatorSymbol = substrings[1],
                Values = substrings[2],
            },
        };
    }
}
