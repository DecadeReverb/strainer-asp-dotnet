using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;
using Fluorite.Strainer.IntegrationTests.Services;
using Microsoft.Extensions.Options;
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

        protected IStrainerContext CreateDefaultContext()
        {
            var options = Options.Create(new StrainerOptions());
            var propertyMapper = new StrainerPropertyMapper(options);
            var propertyMetadataProvider = new StrainerPropertyMetadataProvider(propertyMapper, options);

            var filterExpressionProvider = new FilterExpressionProvider();
            var filterOperatorValidator = new FilterOperatorValidator();
            var filterOperatorMapper = new FilterOperatorMapper(filterOperatorValidator);
            var filterOperatorParser = new FilterOperatorParser(filterOperatorMapper);
            var filterTermParser = new FilterTermParser(filterOperatorParser, filterOperatorMapper);
            var filteringContext = new FilteringContext(filterExpressionProvider, filterOperatorMapper, filterOperatorParser, filterOperatorValidator, filterTermParser);

            var sortExpressionProvider = new SortExpressionProvider(propertyMapper, propertyMetadataProvider);
            var sortingWayFormatter = new SortingWayFormatter();
            var sortTermParser = new SortTermParser(sortingWayFormatter);
            var sortingContext = new SortingContext(sortExpressionProvider, sortingWayFormatter, sortTermParser);

            var customFilterMethodMapper = new CustomFilterMethodMapper(options);
            var customFilterMethodProvider = new ApplicationCustomFilterMethodProvider(customFilterMethodMapper);

            var customSortMethodMapper = new CustomSortMethodMapper(options);
            var customSortMethodProvider = new ApplicationCustomSortMethodProvider(customSortMethodMapper);

            var customMethodsContext = new CustomMethodsContext(customFilterMethodProvider, customSortMethodProvider);

            return new StrainerContext(
                options,
                filteringContext,
                sortingContext,
                propertyMapper,
                propertyMetadataProvider,
                customMethodsContext);
        }
    }
}