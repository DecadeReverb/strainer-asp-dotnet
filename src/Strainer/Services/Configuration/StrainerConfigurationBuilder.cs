using Fluorite.Extensions;
using Fluorite.Strainer.Models.Configuration;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.Services.Configuration
{
    public class StrainerConfigurationBuilder : IStrainerConfigurationBuilder
    {
        private readonly IStrainerOptionsProvider _optionsProvider;
        private readonly IFilterOperatorValidator _filterOperatorValidator;
        private readonly IStrainerModuleLoader _strainerModuleLoader;

        public StrainerConfigurationBuilder(
            IStrainerOptionsProvider optionsProvider,
            IFilterOperatorValidator filterOperatorValidator,
            IStrainerModuleLoader strainerModuleLoader)
        {
            _optionsProvider = optionsProvider;
            _filterOperatorValidator = filterOperatorValidator;
            _strainerModuleLoader = strainerModuleLoader;
        }

        public IStrainerConfiguration Build(IReadOnlyCollection<Type> moduleTypes)
        {
            var validModuleTypes = moduleTypes
                .Where(type => !type.IsAbstract && typeof(IStrainerModule).IsAssignableFrom(type));

            var invalidModuleTypes = moduleTypes.Except(validModuleTypes);
            if (invalidModuleTypes.Any())
            {
                throw new InvalidOperationException(
                    string.Format(
                        "Valid Strainer module must be a non-abstract class implementing `{0}`. " +
                        "Invalid types:\n{1}",
                        typeof(IStrainerModule).FullName,
                        string.Join("\n", invalidModuleTypes.Select(invalidType => invalidType.FullName))));
            }

            var modules = validModuleTypes
                .Select(type => CreateModuleInstance(type))
                .Where(instance => instance != null)
                .ToList();

            var options = _optionsProvider.GetStrainerOptions();

            modules.ForEach(strainerModule => _strainerModuleLoader.Load(strainerModule, options));

            var customFilerMethods = modules
                .SelectMany(module => module
                    .CustomFilterMethods
                    .Select(pair =>
                        new KeyValuePair<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>(
                            pair.Key, pair.Value.ToReadOnly())))
                .Merge()
                .ToReadOnly();
            var customSortMethods = modules
                .SelectMany(module => module
                .CustomSortMethods
                .Select(pair =>
                    new KeyValuePair<Type, IReadOnlyDictionary<string, ICustomSortMethod>>(
                        pair.Key, pair.Value.ToReadOnly())))
                .Merge()
                .ToReadOnly();
            var defaultMetadata = modules
                .SelectMany(module => module.DefaultMetadata)
                .Merge()
                .ToReadOnly();
            var filterOperators = modules
                .SelectMany(module => module.FilterOperators)
                .Union(FilterOperatorMapper.DefaultOperators)
                .Merge()
                .ToReadOnly();
            var objectMetadata = modules
                .SelectMany(module => module.ObjectMetadata.ToReadOnly())
                .Merge()
                .ToReadOnly();
            var propertyMetadata = modules
                .SelectMany(module => module
                .PropertyMetadata
                .Select(pair =>
                    new KeyValuePair<Type, IReadOnlyDictionary<string, IPropertyMetadata>>(
                        pair.Key, pair.Value.ToReadOnly())))
                .Merge()
                .ToReadOnly();

            var compiledConfiguration = new StrainerConfiguration(
                customFilerMethods,
                customSortMethods,
                defaultMetadata,
                filterOperators,
                objectMetadata,
                propertyMetadata);

            // TODO:
            // Make validation optional?
            _filterOperatorValidator.Validate(filterOperators.Values);

            return compiledConfiguration;
        }

        private IStrainerModule CreateModuleInstance(Type type)
        {
            try
            {
                return Activator.CreateInstance(type) as IStrainerModule;
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException(
                    $"Unable to create instance of {type}. " +
                    $"Ensure that type provides parameterless constructor.",
                    exception);
            }
        }
    }
}
