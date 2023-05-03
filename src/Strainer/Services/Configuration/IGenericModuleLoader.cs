using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration
{
    public interface IGenericModuleLoader
    {
        void Load(IStrainerModule strainerModule, StrainerOptions options);
    }
}
