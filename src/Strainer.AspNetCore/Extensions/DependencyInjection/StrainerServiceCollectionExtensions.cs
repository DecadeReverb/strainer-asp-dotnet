using Fluorite.Strainer.AspNetCore.Services;
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

        public static IStrainerBuilder AddStrainer<TProcessor>(
            this IServiceCollection services,
            ServiceLifetime serviceLifetime = DefaultServiceLifetime)
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
                services.AddOptions<StrainerOptions>();
            }

            services.Add<IStrainerOptionsProvider, AspNetCoreStrainerOptionsProvider>(serviceLifetime);

            services.Add<IFilterExpressionProvider, FilterExpressionProvider>(serviceLifetime);
            services.Add<IFilterOperatorMapper, FilterOperatorMapper>(serviceLifetime);
            services.Add<IFilterOperatorParser, FilterOperatorParser>(serviceLifetime);
            services.Add<IFilterOperatorValidator, FilterOperatorValidator>(serviceLifetime);
            services.Add<IFilterTermParser, FilterTermParser>(serviceLifetime);
            services.Add<IFilterContext, FilterContext>(serviceLifetime);

            services.Add<ISortExpressionProvider, SortExpressionProvider>(serviceLifetime);
            services.Add<ISortExpressionValidator, SortExpressionValidator>(serviceLifetime);
            services.Add<ISortingWayFormatter, SortingWayFormatter>(serviceLifetime);
            services.Add<ISortTermParser, SortTermParser>(serviceLifetime);
            services.Add<ISortingContext, SortingContext>(serviceLifetime);

            services.Add<ICustomFilterMethodMapper, CustomFilterMethodMapper>(serviceLifetime);
            services.Add<ICustomSortMethodMapper, CustomSortMethodMapper>(serviceLifetime);
            services.Add<ICustomMethodsContext, CustomMethodsContext>(serviceLifetime);

            services.Add<IPropertyMapper, PropertyMapper>(serviceLifetime);
            services.Add<IAttributePropertyMetadataProvider, AttributePropertyMetadataProvider>(serviceLifetime);
            services.Add<IStrainerContext, StrainerContext>(serviceLifetime);
            services.Add<IStrainerProcessor, TProcessor>(serviceLifetime);

            return new StrainerBuilder(services);
        }

        public static IStrainerBuilder AddStrainer<TProcessor>(
            this IServiceCollection services,
            IConfiguration configuration,
            ServiceLifetime serviceLifetime = DefaultServiceLifetime)
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

            services.Configure<StrainerOptions>(configuration);

            var builder = services.AddStrainer<TProcessor>();

            return builder;
        }

        public static IStrainerBuilder AddStrainer<TProcessor>(
            this IServiceCollection services,
            Action<StrainerOptions> configure,
            ServiceLifetime serviceLifetime = DefaultServiceLifetime)
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

            services.AddOptions<StrainerOptions>().Configure(configure);

            var builder = services.AddStrainer<TProcessor>();

            return builder;
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
    }
}
