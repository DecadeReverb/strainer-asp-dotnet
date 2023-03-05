using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Strainer.IntegrationTests.Fixtures
{
    public static class StrainerFactoryExtensions
    {
        public static IStrainerProcessor CreateProcessorWithSortingWayFormatter<TFormatter>(
            this StrainerFactory factory)
            where TFormatter : class, ISortingWayFormatter, new()
        {
            var customSortingWayFormatter = new TFormatter();

            return factory.CreateProcessor(context =>
            {
                var newSortingContext = new SortingContext(
                    context.Sorting.ExpressionProvider,
                    context.Sorting.ExpressionValidator,
                    customSortingWayFormatter,
                    new SortTermParser(
                        customSortingWayFormatter,
                        factory.CreateOptionsProvider()));
                var newContext = new StrainerContext(
                    context.CustomMethods,
                    factory.CreateOptionsProvider(),
                    context.Filter,
                    newSortingContext,
                    context.Metadata,
                    context.Pipeline);

                return new StrainerProcessor(newContext);
            });
        }
    }
}
