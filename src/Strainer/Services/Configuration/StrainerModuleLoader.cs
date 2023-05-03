using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration
{
    public class StrainerModuleLoader : IStrainerModuleLoader
    {
        private readonly IPropertyInfoProvider _propertyInfoProvider;
        private readonly IGenericModuleLoader _genericModuleLoader;

        public StrainerModuleLoader(
            IPropertyInfoProvider propertyInfoProvider,
            IGenericModuleLoader genericModuleLoader)
        {
            _propertyInfoProvider = propertyInfoProvider;
            _genericModuleLoader = genericModuleLoader;
        }

        public void Load(IStrainerModule strainerModule, StrainerOptions options)
        {
            var isGeneric = strainerModule
                .GetType()
                .GetInterfaces()
                .Any(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStrainerModule<>));

            if (isGeneric)
            {
                _genericModuleLoader.Load(strainerModule, options);
            }
            else
            {
                var moduleBuilder = new StrainerModuleBuilder(_propertyInfoProvider, strainerModule, options);

                strainerModule.Load(moduleBuilder);
            }
        }
    }
}
