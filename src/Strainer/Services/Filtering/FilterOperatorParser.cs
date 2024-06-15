using Fluorite.Strainer.Models.Filtering.Operators;

namespace Fluorite.Strainer.Services.Filtering;

public class FilterOperatorParser : IFilterOperatorParser
{
    private readonly IConfigurationFilterOperatorsProvider _filterOperatorsConfigurationProvider;

    public FilterOperatorParser(IConfigurationFilterOperatorsProvider filterOperatorsConfigurationProvider)
    {
        _filterOperatorsConfigurationProvider = filterOperatorsConfigurationProvider
            ?? throw new ArgumentNullException(nameof(filterOperatorsConfigurationProvider));
    }

    public virtual IFilterOperator GetParsedOperator(string symbol)
    {
        if (string.IsNullOrWhiteSpace(symbol))
        {
            return null;
        }

        _filterOperatorsConfigurationProvider.GetFilterOperators().TryGetValue(symbol, out var filterOperator);

        return filterOperator;
    }
}
