using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace Fluorite.Extensions.DependencyInjection
{
    public static class StrainerServiceCollectionExtensions
    {
        public const ServiceLifetime DefaultServiceLifetime = ServiceLifetime.Scoped;

        public static IStrainerBuilder AddStrainer<TProcessor>(this IServiceCollection services, IConfiguration configuration)
            where TProcessor : class, IStrainerProcessor
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            var options = new StrainerOptions();
            configuration.Bind(options);
            services.AddSingleton(options);

            services.AddOptions<AspNetCoreStrainerOptions>();
            services.Configure<AspNetCoreStrainerOptions>(configuration);

            var builder = services.AddStrainer<TProcessor>();

            return builder;
        }

        public static IStrainerBuilder AddStrainer<TProcessor>(this IServiceCollection services, Action<AspNetCoreStrainerOptions> configure)
            where TProcessor : class, IStrainerProcessor
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (configure == null)
            {
                throw new ArgumentNullException(nameof(configure));
            }

            var options = new AspNetCoreStrainerOptions();
            configure(options);
            services.AddSingleton<StrainerOptions>(options);

            services.AddOptions<AspNetCoreStrainerOptions>().Configure(configure);

            var builder = services.AddStrainer<TProcessor>();

            return builder;
        }

        public static IStrainerBuilder AddStrainer<TProcessor>(this IServiceCollection services)
            where TProcessor : class, IStrainerProcessor
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (services.Any(d => d.ServiceType == typeof(IStrainerProcessor)))
            {
                throw new InvalidOperationException(
                    $"Unable to registrer {nameof(IStrainerProcessor)} " +
                    $"because there is already registered one.");
            }

            // Add Strainer options only if they weren't configured yet.
            if (!services.ContainsServiceOfType<StrainerOptions>())
            {
                services.AddOptions();
                services.AddSingleton<StrainerOptions>();
            }

            var serviceLifetime = services.GetStrainerLifetime();

            services.Add<IFilterExpressionProvider, FilterExpressionProvider>(serviceLifetime);
            services.Add<IFilterOperatorMapper, FilterOperatorMapper>(serviceLifetime);
            services.Add<IFilterOperatorParser, FilterOperatorParser>(serviceLifetime);
            services.Add<IFilterOperatorValidator, FilterOperatorValidator>(serviceLifetime);
            services.Add<IFilterTermParser, FilterTermParser>(serviceLifetime);
            services.Add<IFilteringContext, FilteringContext>(serviceLifetime);

            services.Add<ISortExpressionProvider, SortExpressionProvider>(serviceLifetime);
            services.Add<ISortExpressionValidator, SortExpressionValidator>(serviceLifetime);
            services.Add<ISortingWayFormatter, SortingWayFormatter>(serviceLifetime);
            services.Add<ISortTermParser, SortTermParser>(serviceLifetime);
            services.Add<ISortingContext, SortingContext>(serviceLifetime);

            services.Add<ICustomFilterMethodMapper, CustomFilterMethodMapper>(serviceLifetime);
            services.Add<ICustomSortMethodMapper, CustomSortMethodMapper>(serviceLifetime);
            services.Add<ICustomMethodsContext, CustomMethodsContext>(serviceLifetime);

            services.Add<IStrainerPropertyMapper, StrainerPropertyMapper>(serviceLifetime);
            services.Add<IStrainerPropertyMetadataProvider, StrainerPropertyMetadataProvider>(serviceLifetime);
            services.Add<IStrainerContext, StrainerContext>(serviceLifetime);
            services.Add<IStrainerProcessor, TProcessor>(serviceLifetime);

            return new StrainerBuilder(services);
        }

        private static void Add<TServiceType, TImplementationType>(this IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            services.Add(new ServiceDescriptor(typeof(TServiceType), typeof(TImplementationType), serviceLifetime));
        }

        private static bool ContainsServiceOfType<TImplementationType>(this IServiceCollection services)
        {
            return services.Any(d => d.ServiceType == typeof(TImplementationType));
        }

        private static bool ContainsServiceOfType(this IServiceCollection services, Type implementationType)
        {
            return services.Any(d => d.ServiceType == implementationType);
        }

        private static ServiceLifetime GetStrainerLifetime(this IServiceCollection services)
        {
            using (var provider = services.BuildServiceProvider())
            {
                return provider.GetService<IOptions<AspNetCoreStrainerOptions>>()?.Value.ServiceLifetime
                    ?? DefaultServiceLifetime;
            }
        }
    }
}
