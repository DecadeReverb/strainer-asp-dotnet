using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Sorting;
using System;

namespace Fluorite.Strainer.IntegrationTests
{
    public class StrainerFactory
    {
        public StrainerFactory()
        {

        }

        public IStrainerProcessor CreateDefaultProcessor()
        {
            var context = CreateDefaultContext();

            return new StrainerProcessor(context);
        }

        public IStrainerProcessor CreateDefaultProcessor(Action<StrainerOptions> configureOptions)
        {
            if (configureOptions == null)
            {
                throw new ArgumentNullException(nameof(configureOptions));
            }

            var context = CreateDefaultContext();
            configureOptions(context.Options);

            return new StrainerProcessor(context);
        }

        public IStrainerProcessor CreateProcessor(Func<IStrainerContext, IStrainerProcessor> function)
        {
            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            var context = CreateDefaultContext();

            return function(context);
        }

        public IntegrationTestsStrainerOptionsProvider CreateOptionsProvider()
        {
            return new IntegrationTestsStrainerOptionsProvider();
        }

        protected IStrainerContext CreateDefaultContext()
        {
            var optionsProvider = new IntegrationTestsStrainerOptionsProvider();
            var propertyMapper = new PropertyMetadataMapper(optionsProvider);
            var attributeMetadataProvider = new AttributeMetadataProvider(optionsProvider);
            var propertyMetadataProviders = new IPropertyMetadataProvider[]
            {
                propertyMapper,
                attributeMetadataProvider
            };
            var mainMetadataProvider = new MainMetadataProvider(propertyMetadataProviders);

            var filterExpressionProvider = new FilterExpressionProvider(optionsProvider);
            var filterOperatorValidator = new FilterOperatorValidator();
            var filterOperatorMapper = new FilterOperatorMapper(filterOperatorValidator);
            var filterOperatorParser = new FilterOperatorParser(filterOperatorMapper);
            var filterTermParser = new FilterTermParser(filterOperatorParser, filterOperatorMapper);
            var filteringContext = new FilterContext(filterExpressionProvider, filterOperatorMapper, filterOperatorParser, filterOperatorValidator, filterTermParser);

            var sortExpressionProvider = new SortExpressionProvider(mainMetadataProvider);
            var sortExpressionValidator = new SortExpressionValidator();
            var sortingWayFormatter = new DescendingPrefixSortingWayFormatter();
            var sortTermParser = new SortTermParser(sortingWayFormatter, optionsProvider);
            var sortingContext = new SortingContext(sortExpressionProvider, sortExpressionValidator, sortingWayFormatter, sortTermParser);

            var customMethodsContext = new CustomMethodsContext(optionsProvider);

            return new StrainerContext(
                optionsProvider,
                filteringContext,
                sortingContext,
                propertyMapper,
                mainMetadataProvider,
                customMethodsContext);
        }
    }
}