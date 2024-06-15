using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration;

public class StrainerModuleLoader : IStrainerModuleLoader
{
    private readonly IModuleLoadingStrategySelector _moduleLoadingStrategySelector;

    public StrainerModuleLoader(IModuleLoadingStrategySelector moduleLoadingStrategySelector)
    {
        _moduleLoadingStrategySelector = Guard.Against.Null(moduleLoadingStrategySelector);
    }

    public void Load(IStrainerModule strainerModule)
    {
        Guard.Against.Null(strainerModule);

        var strategy = _moduleLoadingStrategySelector.Select(strainerModule);

        strategy.Load(strainerModule);
    }
}
