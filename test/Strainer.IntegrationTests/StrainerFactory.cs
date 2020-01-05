using Fluorite.Extensions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Modules;
using Fluorite.Strainer.Services.Sorting;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Fluorite.Strainer.IntegrationTests
{
    public class StrainerFactory
    {
        public StrainerFactory()
        {

        }

        public IStrainerProcessor CreateDefaultProcessor<TModule>()
            where TModule : StrainerModule
        {
            return CreateDefaultProcessor(typeof(TModule));
        }

        public IStrainerProcessor CreateDefaultProcessor(params Type[] strainerModuleTypes)
        {
            var context = CreateDefaultContext(strainerModuleTypes);

            return new StrainerProcessor(context);
        }

        public IStrainerProcessor CreateDefaultProcessor<TModule>(Action<StrainerOptions> configureOptions)
            where TModule : StrainerModule
        {
            return CreateDefaultProcessor(configureOptions, typeof(TModule));
        }

        public IStrainerProcessor CreateDefaultProcessor(Action<StrainerOptions> configureOptions, params Type[] strainerModuleTypes)
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            var context = CreateDefaultContext(strainerModuleTypes);
            configureOptions(context.Options);

            return new StrainerProcessor(context);
        }

        public IStrainerProcessor CreateProcessor<TModule>(Func<IStrainerContext, IStrainerProcessor> function)
            where TModule : StrainerModule
        {
            return CreateProcessor(function, typeof(TModule));
        }

        public IStrainerProcessor CreateProcessor(Func<IStrainerContext, IStrainerProcessor> function, params Type[] strainerModuleTypes)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            var context = CreateDefaultContext(strainerModuleTypes);

            return function(context);
        }

        public IntegrationTestsStrainerOptionsProvider CreateOptionsProvider()
        {
            return new IntegrationTestsStrainerOptionsProvider();
        }

        protected IStrainerContext CreateDefaultContext(params Type[] strainerModuleTypes)
        {
            if (strainerModuleTypes is null)
            {
                throw new ArgumentNullException(nameof(strainerModuleTypes));
            }

            var optionsProvider = new IntegrationTestsStrainerOptionsProvider();

            var modules = strainerModuleTypes
                .Select(type => Activator.CreateInstance(type) as StrainerModule)
                .Where(instance => instance != null)
                .ToList();

            modules.ForEach(strainerModule =>
            {
                strainerModule.Options = optionsProvider.GetStrainerOptions();
                strainerModule.Load();
            });

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

            var fluentApiMetadataProvider = new FluentApiMetadataProvider(
                optionsProvider,
                defaultMetadata,
                objectMetadata,
                propertyMetadata);
            var attributeMetadataProvider = new AttributeMetadataProvider(optionsProvider);
            var propertyMetadataProviders = new IMetadataProvider[]
            {
                fluentApiMetadataProvider,
                attributeMetadataProvider
            };
            var mainMetadataProvider = new MetadataFacade(propertyMetadataProviders);

            var filterExpressionProvider = new FilterExpressionProvider(optionsProvider);
            var filterOperatorValidator = new FilterOperatorValidator();
            var filterOperatorParser = new FilterOperatorParser(filterOperators);
            var filterTermParser = new FilterTermParser(
                filterOperatorParser,
                filterOperators);
            var filteringContext = new FilterContext(
                filterExpressionProvider,
                filterOperators,
                filterOperatorParser,
                filterOperatorValidator,
                filterTermParser);

            var sortExpressionProvider = new SortExpressionProvider(mainMetadataProvider);
            var sortExpressionValidator = new SortExpressionValidator();
            var sortingWayFormatter = new DescendingPrefixSortingWayFormatter();
            var sortTermParser = new SortTermParser(sortingWayFormatter, optionsProvider);
            var sortingContext = new SortingContext(
                sortExpressionProvider,
                sortExpressionValidator,
                sortingWayFormatter,
                sortTermParser);

            var customMethodsContext = new CustomMethodsContext(
                customFilerMethods,
                customSortMethods);

            return new StrainerContext(
                optionsProvider,
                filteringContext,
                sortingContext,
                mainMetadataProvider,
                customMethodsContext);
        }
    }
}