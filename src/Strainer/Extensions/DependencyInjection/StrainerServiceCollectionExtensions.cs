using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;

namespace Fluorite.Extensions.DependencyInjection
{
    public static class StrainerServiceCollectionExtensions
    {
        private const ServiceLifetime StrainerServicesLifetime = ServiceLifetime.Scoped;

        public static IStrainerBuilder AddCustomFilterMethods<TFilterMethods>(this IStrainerBuilder builder)
            where TFilterMethods : class, IStrainerCustomFilterMethods
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.TryAddService<IStrainerCustomFilterMethods, TFilterMethods>(StrainerServicesLifetime);

            return builder;
        }

        public static IStrainerBuilder AddCustomSortMethods<TSortMethods>(this IStrainerBuilder builder)
            where TSortMethods : class, IStrainerCustomSortMethods
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.TryAddService<IStrainerCustomSortMethods, TSortMethods>(StrainerServicesLifetime);

            return builder;
        }

        public static IStrainerBuilder AddStrainer<TProcessor>(this IServiceCollection services, Action<StrainerOptions> options)
            where TProcessor : class, IStrainerProcessor
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var builder = services.AddStrainer<TProcessor>();
            builder.Services.PostConfigure(options);

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

            using (var provider = services.BuildServiceProvider())
            {
                // Add Strainer options only if they weren't configured yet.
                if (!services.ContainsServiceOfType<IOptions<StrainerOptions>>())
                {
                    var configuration = provider.GetRequiredService<IConfiguration>();
                    services.Configure<StrainerOptions>(configuration.GetSection("Strainer"));
                }
            }

            services.TryAddService<IFilterOperatorParser, FilterOperatorParser>(StrainerServicesLifetime);
            services.TryAddService<IFilterOperatorProvider, FilterOperatorProvider>(StrainerServicesLifetime);
            services.TryAddService<IFilterOperatorValidator, FilterOperatorValidator>(StrainerServicesLifetime);
            services.TryAddService<IFilterTermParser, FilterTermParser>(StrainerServicesLifetime);
            services.TryAddService<IFilteringContext, FilteringContext>(StrainerServicesLifetime);

            services.TryAddService<ISortTermParser, SortTermParser>(StrainerServicesLifetime);
            services.TryAddService<ISortingContext, SortingContext>(StrainerServicesLifetime);

            services.TryAddService<IStrainerPropertyMapper, StrainerPropertyMapper>(StrainerServicesLifetime);

            services.TryAddService<IStrainerCustomMethodsContext, StrainerCustomMethodsContext>(StrainerServicesLifetime);

            services.TryAddService<IStrainerContext, StrainerContext>(StrainerServicesLifetime);

            services.Add<IStrainerProcessor, TProcessor>(StrainerServicesLifetime);

            return new StrainerBuilder(services);
        }

        private static IServiceCollection Add<TServiceType, TImplementationType>(this IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            services.Add(new ServiceDescriptor(typeof(TServiceType), typeof(TImplementationType), serviceLifetime));

            return services;
        }

        private static bool ContainsServiceOfType<TImplementationType>(this IServiceCollection services)
        {
            return services.Any(d => d.ServiceType == typeof(TImplementationType));
        }

        private static void TryAddService<TServiceType, TImplementationType>(this IServiceCollection services, ServiceLifetime serviceLifetime)
        {
            if (!services.ContainsServiceOfType<TServiceType>())
            {
                services.Add<TServiceType, TImplementationType>(serviceLifetime);
            }
        }
    }
}
