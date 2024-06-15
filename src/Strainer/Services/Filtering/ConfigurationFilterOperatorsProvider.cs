using Fluorite.Strainer.Models.Filtering.Operators;
using Fluorite.Strainer.Services.Configuration;

namespace Fluorite.Strainer.Services.Filtering;

public class ConfigurationFilterOperatorsProvider : IConfigurationFilterOperatorsProvider
{
    private readonly IStrainerConfigurationProvider _strainerConfigurationProvider;

    public ConfigurationFilterOperatorsProvider(IStrainerConfigurationProvider strainerConfigurationProvider)
    {
        _strainerConfigurationProvider = strainerConfigurationProvider
            ?? throw new ArgumentNullException(nameof(strainerConfigurationProvider));
    }

    /// <summary>
    /// Gets the object filter operator dictionary.
    /// </summary>w
    public IReadOnlyDictionary<string, IFilterOperator> GetFilterOperators()
    {
        return _strainerConfigurationProvider
            .GetStrainerConfiguration()
            .FilterOperators;
    }
}
