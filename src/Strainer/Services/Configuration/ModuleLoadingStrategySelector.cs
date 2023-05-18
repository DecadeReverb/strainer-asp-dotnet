using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration
{
    public class ModuleLoadingStrategySelector : IModuleLoadingStrategySelector
    {
        private readonly IGenericModuleLoadingStrategy _genericModuleLoadingStrategy;
        private readonly IPlainModuleLoadingStrategy _plainModuleLoadingStrategy;

        public ModuleLoadingStrategySelector(
            IGenericModuleLoadingStrategy genericModuleLoadingStrategy,
            IPlainModuleLoadingStrategy plainModuleLoadingStrategy)
        {
            _genericModuleLoadingStrategy = genericModuleLoadingStrategy ?? throw new ArgumentNullException(nameof(genericModuleLoadingStrategy));
            _plainModuleLoadingStrategy = plainModuleLoadingStrategy ?? throw new ArgumentNullException(nameof(plainModuleLoadingStrategy));
        }

        public IModuleLoadingStrategy Select(IStrainerModule strainerModule)
        {
            if (strainerModule is null)
            {
                throw new ArgumentNullException(nameof(strainerModule));
            }

            var isGeneric = strainerModule
                .GetType()
                .GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStrainerModule<>));

            return isGeneric
                ? _genericModuleLoadingStrategy
                : _plainModuleLoadingStrategy;
        }
    }
}
