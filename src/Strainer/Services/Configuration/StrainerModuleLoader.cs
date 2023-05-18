using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration
{
    public class StrainerModuleLoader : IStrainerModuleLoader
    {
        private readonly IModuleLoadingStrategySelector _moduleLoadingStrategySelector;

        public StrainerModuleLoader(IModuleLoadingStrategySelector moduleLoadingStrategySelector)
        {
            _moduleLoadingStrategySelector = moduleLoadingStrategySelector ?? throw new ArgumentNullException(nameof(moduleLoadingStrategySelector));
        }

        public void Load(IStrainerModule strainerModule)
        {
            if (strainerModule is null)
            {
                throw new ArgumentNullException(nameof(strainerModule));
            }

            var strategy = _moduleLoadingStrategySelector.Select(strainerModule);

            strategy.Load(strainerModule);
        }
    }
}
