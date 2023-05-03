using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration
{
    public class GenericModuleLoader : IGenericModuleLoader
    {
        private readonly IPropertyInfoProvider _propertyInfoProvider;

        public GenericModuleLoader(IPropertyInfoProvider propertyInfoProvider)
        {
            _propertyInfoProvider = propertyInfoProvider ?? throw new ArgumentNullException(nameof(propertyInfoProvider));
        }

        public void Load(IStrainerModule strainerModule, StrainerOptions options)
        {
            if (strainerModule is null)
            {
                throw new ArgumentNullException(nameof(strainerModule));
            }

            if (options is null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var genericStrainerModuleInterfaceType = strainerModule
                .GetType()
                .GetInterfaces()
                .FirstOrDefault(i => i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStrainerModule<>));
            var moduleGenericType = genericStrainerModuleInterfaceType.GetGenericArguments().First();
            var builderType = typeof(StrainerModuleBuilder<>).MakeGenericType(moduleGenericType);
            var builder = Activator.CreateInstance(builderType, _propertyInfoProvider, strainerModule, options);
            var method = genericStrainerModuleInterfaceType.GetMethod(nameof(IStrainerModule<object>.Load));

            method.Invoke(strainerModule, new[] { builder });
        }
    }
}
