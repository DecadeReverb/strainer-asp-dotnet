using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration;

public interface IModuleLoadingStrategy
{
    void Load(IStrainerModule strainerModule);
}
