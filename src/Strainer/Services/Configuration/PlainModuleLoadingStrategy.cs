using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration;

public class PlainModuleLoadingStrategy : IPlainModuleLoadingStrategy, IModuleLoadingStrategy
{
    private readonly IPropertyInfoProvider _propertyInfoProvider;
    private readonly IStrainerOptionsProvider _strainerOptionsProvider;

    public PlainModuleLoadingStrategy(
        IPropertyInfoProvider propertyInfoProvider,
        IStrainerOptionsProvider strainerOptionsProvider)
    {
        _propertyInfoProvider = Guard.Against.Null(propertyInfoProvider);
        _strainerOptionsProvider = Guard.Against.Null(strainerOptionsProvider);
    }

    public void Load(IStrainerModule strainerModule)
    {
        Guard.Against.Null(strainerModule);

        // TODO: Use factory for builder?
        var options = _strainerOptionsProvider.GetStrainerOptions();
        var builder = new StrainerModuleBuilder(_propertyInfoProvider, strainerModule, options);

        strainerModule.Load(builder);
    }
}
