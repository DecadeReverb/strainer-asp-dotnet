using Fluorite.Strainer.Models.Configuration;
using Fluorite.Strainer.Services.Validation;

namespace Fluorite.Strainer.Services.Configuration
{
    public class StrainerConfigurationFactory : IStrainerConfigurationFactory
    {
        private readonly IStrainerOptionsProvider _optionsProvider;
        private readonly IFilterOperatorValidator _filterOperatorValidator;
        private readonly IStrainerModuleLoader _strainerModuleLoader;
        private readonly IStrainerModuleFactory _strainerModuleFactory;
        private readonly IStrainerModuleTypeValidator _strainerModuleTypeValidator;

        public StrainerConfigurationFactory(
            IStrainerOptionsProvider optionsProvider,
            IFilterOperatorValidator filterOperatorValidator,
            IStrainerModuleLoader strainerModuleLoader,
            IStrainerModuleFactory strainerModuleFactory,
            IStrainerModuleTypeValidator strainerModuleTypeValidator)
        {
            _optionsProvider = optionsProvider;
            _filterOperatorValidator = filterOperatorValidator;
            _strainerModuleLoader = strainerModuleLoader;
            _strainerModuleFactory = strainerModuleFactory;
            _strainerModuleTypeValidator = strainerModuleTypeValidator;
        }

        public IStrainerConfiguration Create(IReadOnlyCollection<Type> moduleTypes)
        {
            var validModuleTypes = _strainerModuleTypeValidator.GetValidModuleTypes(moduleTypes);
            var options = _optionsProvider.GetStrainerOptions();
            var modules = validModuleTypes
                .Select(type => _strainerModuleFactory.CreateModule(type))
                .Where(instance => instance != null)
                .ToList();

            modules.ForEach(strainerModule => _strainerModuleLoader.Load(strainerModule, options));

            var configuration = new StrainerConfigurationBuilder()
                .WithPropertyMetadata(modules)
                .WithDefaultMetadata(modules)
                .WithObjectMetadata(modules)
                .WithFilterOperators(modules)
                .WithCustomFilterMethods(modules)
                .WithCustomSortMethods(modules)
                .Build();

            // TODO:
            // Make validation optional?
            _filterOperatorValidator.Validate(configuration.FilterOperators.Values);

            return configuration;
        }
    }
}
