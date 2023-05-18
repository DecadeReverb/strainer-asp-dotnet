using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration
{
    public interface IStrainerModuleLoader
    {
        void Load(IStrainerModule strainerModule);
    }
}
