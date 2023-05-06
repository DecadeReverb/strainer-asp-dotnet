using Fluorite.Extensions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Configuration;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Metadata;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Configuration;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Metadata.Attributes;
using Fluorite.Strainer.Services.Metadata.FluentApi;
using Fluorite.Strainer.Services.Modules;
using Fluorite.Strainer.Services.Pagination;
using Fluorite.Strainer.Services.Pipelines;
using Fluorite.Strainer.Services.Sorting;
using Fluorite.Strainer.Services.Validation;

namespace Fluorite.Strainer.IntegrationTests.Fixtures
{
    public class StrainerFactory
    {
        public StrainerFactory()
        {

        }

        public IStrainerProcessor CreateDefaultProcessor<TModule>()
            where TModule : IStrainerModule
        {
            return CreateDefaultProcessor(typeof(TModule));
        }

        public IStrainerProcessor CreateDefaultProcessor(params Type[] strainerModuleTypes)
        {
            var context = CreateDefaultContext(strainerModuleTypes);

            return new StrainerProcessor(context);
        }

        public IStrainerProcessor CreateDefaultProcessor<TModule>(Action<StrainerOptions> configureOptions)
            where TModule : IStrainerModule
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
            where TModule : IStrainerModule
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
            var propertyInfoProvider = new PropertyInfoProvider();

            var modules = strainerModuleTypes
                .Select(type => Activator.CreateInstance(type) as IStrainerModule)
                .Where(instance => instance != null)
                .ToList();

            modules.ForEach(strainerModule => LoadModule(strainerModule, optionsProvider, propertyInfoProvider));

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

            var strainerConfiguration = new StrainerConfiguration(
                customFilerMethods,
                customSortMethods,
                defaultMetadata,
                filterOperators,
                objectMetadata,
                propertyMetadata);

            var strainerConfigurationProvider = new StrainerConfigurationProvider(strainerConfiguration);
            var configurationMetadataProvider = new ConfigurationMetadataProvider(strainerConfigurationProvider);
            var configurationFilterOperatorsProvider = new ConfigurationFilterOperatorsProvider(strainerConfigurationProvider);
            var configurationCustomFilterMethodsProvider = new ConfigurationCustomMethodsProvider(strainerConfigurationProvider);

            var metadataSourceTypeProvider = new MetadataSourceTypeProvider();
            var metadataAssemblySourceProvider = new AppDomainAssemblySourceProvider();
            var metadataSourceChecker = new MetadataSourceChecker(optionsProvider);
            var strainerObjectAttributeProvider = new StrainerObjectAttributeProvider();
            var strainerPropertyAttributeProvider = new StrainerPropertyAttributeProvider();
            var attributePropertyMetadataBuilder = new AttributePropertyMetadataBuilder();
            var objectMetadataProvider = new ObjectMetadataProvider(optionsProvider, attributePropertyMetadataBuilder);
            var propertyMetadataDictionaryProvider = new PropertyMetadataDictionaryProvider(
                propertyInfoProvider,
                strainerPropertyAttributeProvider,
                attributePropertyMetadataBuilder);
            var attributeMetadataRetriever = new AttributeMetadataRetriever(
                metadataSourceChecker,
                attributePropertyMetadataBuilder,
                strainerObjectAttributeProvider,
                strainerPropertyAttributeProvider,
                propertyInfoProvider);

            var attributeMetadataProvider = new AttributeMetadataProvider(
                metadataSourceTypeProvider,
                metadataAssemblySourceProvider,
                objectMetadataProvider,
                attributeMetadataRetriever,
                strainerObjectAttributeProvider,
                propertyMetadataDictionaryProvider);

            var fluentApiMetadataProvider = new FluentApiMetadataProvider(
                optionsProvider,
                configurationMetadataProvider);

            var propertyMetadataProviders = new IMetadataProvider[]
            {
                fluentApiMetadataProvider,
                attributeMetadataProvider
            };
            var metadataFacade = new MetadataFacade(propertyMetadataProviders);

            var filterExpressionProvider = new FilterExpressionProvider(optionsProvider);
            var filterOperatorValidator = new FilterOperatorValidator();
            var filterOperatorParser = new FilterOperatorParser(configurationFilterOperatorsProvider);
            var filterTermNamesParser = new FilterTermNamesParser();
            var filterTermValuesParser = new FilterTermValuesParser();
            var filterTermSectionsParser = new FilterTermSectionsParser(configurationFilterOperatorsProvider);
            var filterTermParser = new FilterTermParser(
                filterOperatorParser,
                filterTermNamesParser,
                filterTermValuesParser,
                filterTermSectionsParser);
            var filteringContext = new FilterContext(
                filterExpressionProvider,
                filterOperatorParser,
                filterOperatorValidator,
                filterTermParser);

            var sortExpressionProvider = new SortExpressionProvider(metadataFacade);
            var sortExpressionValidator = new SortExpressionValidator();
            var sortingWayFormatter = new DescendingPrefixSortingWayFormatter();
            var sortTermParser = new SortTermParser(sortingWayFormatter, optionsProvider);
            var customSortingApplier = new CustomSortingApplier(configurationCustomFilterMethodsProvider);
            var sortingApplier = new SortingApplier(customSortingApplier);
            var sortingContext = new SortingContext(
                sortExpressionProvider,
                sortExpressionValidator,
                sortingWayFormatter,
                sortTermParser);

            var pageNumberEvaluator = new PageNumberEvaluator(optionsProvider);
            var filterPipelineOperation = new FilterPipelineOperation();
            var sortPipelineOperation = new SortPipelineOperation(sortingApplier);
            var pageSizeEvaluator = new PageSizeEvaluator(optionsProvider);
            var paginatePipelineOperation = new PaginatePipelineOperation(pageNumberEvaluator, pageSizeEvaluator);
            var pipelineBuilderFactory = new StrainerPipelineBuilderFactory(
                filterPipelineOperation,
                sortPipelineOperation,
                paginatePipelineOperation);
            var pipelineContext = new PipelineContext(pipelineBuilderFactory);

            return new StrainerContext(
                configurationCustomFilterMethodsProvider,
                optionsProvider,
                filteringContext,
                sortingContext,
                metadataFacade,
                pipelineContext);
        }

        private void LoadModule(
            IStrainerModule strainerModule,
            IntegrationTestsStrainerOptionsProvider optionsProvider,
            PropertyInfoProvider propertyInfoProvider)
        {
            var options = optionsProvider.GetStrainerOptions();
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