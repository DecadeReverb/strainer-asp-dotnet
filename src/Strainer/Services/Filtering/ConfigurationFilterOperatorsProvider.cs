using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Services.Configuration;

namespace Fluorite.Strainer.Services.Filtering;

public class ConfigurationFilterOperatorsProvider : IConfigurationFilterOperatorsProvider
{
    private readonly IStrainerConfigurationProvider _strainerConfigurationProvider;

    public ConfigurationFilterOperatorsProvider(IStrainerConfigurationProvider strainerConfigurationProvider)
    {
        _strainerConfigurationProvider = Guard.Against.Null(strainerConfigurationProvider);
    }

    /// <inheritdoc/>
    public IReadOnlyDictionary<string, IFilterOperator> GetFilterOperators()
    {
        return _strainerConfigurationProvider
            .GetStrainerConfiguration()
            .FilterOperators;
    }
}
