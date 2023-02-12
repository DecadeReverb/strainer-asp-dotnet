using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Modules;
using System;
using System.Linq;

namespace Fluorite.Strainer.Services.Configuration
{
    public class StrainerModuleLoader : IStrainerModuleLoader
    {
        private readonly IPropertyInfoProvider _propertyInfoProvider;

        public StrainerModuleLoader(IPropertyInfoProvider propertyInfoProvider)
        {
            _propertyInfoProvider = propertyInfoProvider;
        }

        public void Load(IStrainerModule strainerModule, StrainerOptions options)
        {
            var genericStrainerModuleInterfaceType = strainerModule
                .GetType()
                .GetInterfaces()
                .FirstOrDefault(i =>
                    i.IsGenericType && i.GetGenericTypeDefinition() == typeof(IStrainerModule<>));

            if (genericStrainerModuleInterfaceType is not null)
            {
                var moduleGenericType = genericStrainerModuleInterfaceType.GetGenericArguments().First();
                var builderType = typeof(StrainerModuleBuilder<>).MakeGenericType(moduleGenericType);
                var builder = Activator.CreateInstance(builderType, _propertyInfoProvider, strainerModule, options);
                var method = genericStrainerModuleInterfaceType.GetMethod(nameof(IStrainerModule<object>.Load));

                method.Invoke(strainerModule, new[] { builder });
            }
            else
            {
                var moduleBuilder = new StrainerModuleBuilder(_propertyInfoProvider, strainerModule, options);

                strainerModule.Load(moduleBuilder);
            }
        }
    }
}
