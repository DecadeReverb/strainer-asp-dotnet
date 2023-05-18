using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration
{
    public class PlainModuleLoadingStrategy : IPlainModuleLoadingStrategy, IModuleLoadingStrategy
    {
        private readonly IPropertyInfoProvider _propertyInfoProvider;
        private readonly IStrainerOptionsProvider _strainerOptionsProvider;

        public PlainModuleLoadingStrategy(
            IPropertyInfoProvider propertyInfoProvider,
            IStrainerOptionsProvider strainerOptionsProvider)
        {
            _propertyInfoProvider = propertyInfoProvider ?? throw new ArgumentNullException(nameof(propertyInfoProvider));
            _strainerOptionsProvider = strainerOptionsProvider ?? throw new ArgumentNullException(nameof(strainerOptionsProvider));
        }

        public void Load(IStrainerModule strainerModule)
        {
            if (strainerModule is null)
            {
                throw new ArgumentNullException(nameof(strainerModule));
            }

            // TODO: Use factory for builder?
            var options = _strainerOptionsProvider.GetStrainerOptions();
            var builder = new StrainerModuleBuilder(_propertyInfoProvider, strainerModule, options);

            strainerModule.Load(builder);
        }
    }
}
