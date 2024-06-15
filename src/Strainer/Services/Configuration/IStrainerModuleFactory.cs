using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration;

public interface IStrainerModuleFactory
{
    IStrainerModule CreateModule(Type moduleType);
}