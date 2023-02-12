using Fluorite.Extensions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Configuration;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Modules;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluorite.Strainer.Services.Configuration
{
    public class StrainerConfigurationBuilder : IStrainerConfigurationBuilder
    {
        private readonly IStrainerOptionsProvider _optionsProvider;
        private readonly IFilterOperatorValidator _filterOperatorValidator;
        private readonly IPropertyInfoProvider _propertyInfoProvider;

        public StrainerConfigurationBuilder(
            IStrainerOptionsProvider optionsProvider,
            IFilterOperatorValidator filterOperatorValidator,
            IPropertyInfoProvider propertyInfoProvider)
        {
            _optionsProvider = optionsProvider;
            _filterOperatorValidator = filterOperatorValidator;
            _propertyInfoProvider = propertyInfoProvider;
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

            modules.ForEach(strainerModule => LoadModule(strainerModule, options, _propertyInfoProvider));

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

        private void LoadModule(
            IStrainerModule strainerModule,
            StrainerOptions options,
            IPropertyInfoProvider propertyInfoProvider)
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
                var builder = Activator.CreateInstance(builderType, propertyInfoProvider, strainerModule, options);
                var method = genericStrainerModuleInterfaceType.GetMethod(nameof(IStrainerModule<object>.Load));

                method.Invoke(strainerModule, new[] { builder });
            }
            else
            {
                var moduleBuilder = new StrainerModuleBuilder(propertyInfoProvider, strainerModule, options);

                strainerModule.Load(moduleBuilder);
            }
        }
    }
}
