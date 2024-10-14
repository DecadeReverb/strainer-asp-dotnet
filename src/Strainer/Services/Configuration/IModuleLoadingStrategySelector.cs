using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration;

public interface IModuleLoadingStrategySelector
{
    IModuleLoadingStrategy Select(IStrainerModule strainerModule);
}
