using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Sorting;
using Microsoft.Extensions.DependencyInjection;

namespace Fluorite.Strainer.IntegrationTests.Fixtures;

public static class StrainerFactoryExtensions
{
    public static IStrainerProcessor CreateProcessorWithSortingWayFormatter<TFormatter>(
        this StrainerFactory factory)
        where TFormatter : class, ISortingWayFormatter, new()
    {
        var customSortingWayFormatter = new TFormatter();

        return factory.CreateProcessor(services =>
        {
            var formatter = new ServiceDescriptor(
                typeof(ISortingWayFormatter),
                typeof(TFormatter),
                StrainerFactory.ServicesLifetime);

            services.Add(formatter);
        });
    }
}
