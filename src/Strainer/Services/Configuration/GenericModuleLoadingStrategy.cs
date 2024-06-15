using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration;

public class GenericModuleLoadingStrategy : IGenericModuleLoadingStrategy, IModuleLoadingStrategy
{
    private readonly IStrainerModuleBuilderFactory _strainerModuleBuilderFactory;

    public GenericModuleLoadingStrategy(IStrainerModuleBuilderFactory strainerModuleBuilderFactory)
    {
        _strainerModuleBuilderFactory = strainerModuleBuilderFactory ?? throw new ArgumentNullException(nameof(strainerModuleBuilderFactory));
    }

    public void Load(IStrainerModule strainerModule)
    {
        if (strainerModule is null)
        {
            throw new ArgumentNullException(nameof(strainerModule));
        }

        var genericStrainerModuleInterfaceType = strainerModule
            .GetType()
            .GetInterfaces()
            .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStrainerModule<>));
        var moduleTypeParameter = genericStrainerModuleInterfaceType.GetGenericArguments().First();
        var builder = _strainerModuleBuilderFactory.Create(moduleTypeParameter, strainerModule);
        var method = genericStrainerModuleInterfaceType.GetMethod(nameof(IStrainerModule<object>.Load));

        method.Invoke(strainerModule, new[] { builder });
    }
}
