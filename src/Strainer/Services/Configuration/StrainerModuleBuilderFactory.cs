using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration
{
    public class StrainerModuleBuilderFactory : IStrainerModuleBuilderFactory
    {
        private readonly IPropertyInfoProvider _propertyInfoProvider;
        private readonly IStrainerOptionsProvider _strainerOptionsProvider;

        public StrainerModuleBuilderFactory(
            IPropertyInfoProvider propertyInfoProvider,
            IStrainerOptionsProvider strainerOptionsProvider)
        {
            _propertyInfoProvider = propertyInfoProvider ?? throw new ArgumentNullException(nameof(propertyInfoProvider));
            _strainerOptionsProvider = strainerOptionsProvider ?? throw new ArgumentNullException(nameof(strainerOptionsProvider));
        }

        public object Create(Type moduleTypeParameter, IStrainerModule strainerModule)
        {
            if (moduleTypeParameter is null)
            {
                throw new ArgumentNullException(nameof(moduleTypeParameter));
            }

            if (strainerModule is null)
            {
                throw new ArgumentNullException(nameof(strainerModule));
            }

            var options = _strainerOptionsProvider.GetStrainerOptions();
            var builderType = typeof(StrainerModuleBuilder<>).MakeGenericType(moduleTypeParameter);

            try
            {
                return Activator.CreateInstance(builderType, _propertyInfoProvider, strainerModule, options);
            }
            catch (Exception ex)
            {
                throw new StrainerException($"Unable to create a module builder for module of type {strainerModule.GetType().FullName}.", ex);
            }
        }
    }
}
