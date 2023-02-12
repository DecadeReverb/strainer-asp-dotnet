using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration
{
    public interface IStrainerModuleLoader
    {
        void Load(IStrainerModule strainerModule, StrainerOptions options);
    }
}
