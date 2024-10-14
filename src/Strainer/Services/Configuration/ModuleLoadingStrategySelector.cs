using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration;

public class ModuleLoadingStrategySelector : IModuleLoadingStrategySelector
{
    private readonly IGenericModuleLoadingStrategy _genericModuleLoadingStrategy;
    private readonly IPlainModuleLoadingStrategy _plainModuleLoadingStrategy;

    public ModuleLoadingStrategySelector(
        IGenericModuleLoadingStrategy genericModuleLoadingStrategy,
        IPlainModuleLoadingStrategy plainModuleLoadingStrategy)
    {
        _genericModuleLoadingStrategy = Guard.Against.Null(genericModuleLoadingStrategy);
        _plainModuleLoadingStrategy = Guard.Against.Null(plainModuleLoadingStrategy);
    }

    public IModuleLoadingStrategy Select(IStrainerModule strainerModule)
    {
        Guard.Against.Null(strainerModule);

        var isGeneric = strainerModule
            .GetType()
            .GetInterfaces()
            .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStrainerModule<>));

        return isGeneric
            ? _genericModuleLoadingStrategy
            : _plainModuleLoadingStrategy;
    }
}
