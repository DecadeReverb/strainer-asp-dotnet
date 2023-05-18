using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration
{
    public interface IStrainerModuleBuilderFactory
    {
        object Create(Type moduleTypeParameter, IStrainerModule strainerModule);
    }
}
