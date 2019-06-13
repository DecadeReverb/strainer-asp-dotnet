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
        public static IStrainerBuilder AddCustomFilterMethods<TFilterMethods>(this IStrainerBuilder builder)
            where TFilterMethods : class, IStrainerCustomFilterMethods
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var options = builder.Services.GetStrainerOptions();
            builder.Services.Add<IStrainerCustomFilterMethods, TFilterMethods>(options.ServiceLifetime);

            return builder;
        }

        public static IStrainerBuilder AddCustomSortMethods<TSortMethods>(this IStrainerBuilder builder)
            where TSortMethods : class, IStrainerCustomSortMethods
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            var options = builder.Services.GetStrainerOptions();
            builder.Services.Add<IStrainerCustomSortMethods, TSortMethods>(options.ServiceLifetime);

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

            var options = services.GetStrainerOptions();

            services.Add<IFilterOperatorParser, FilterOperatorParser>(options.ServiceLifetime);
            services.Add<IFilterOperatorProvider, FilterOperatorProvider>(options.ServiceLifetime);
            services.Add<IFilterOperatorValidator, FilterOperatorValidator>(options.ServiceLifetime);
            services.Add<IFilterTermParser, FilterTermParser>(options.ServiceLifetime);
            services.Add<IFilteringContext, FilteringContext>(options.ServiceLifetime);

            services.Add<ISortingWayFormatter, SortingWayFormatter>(options.ServiceLifetime);
            services.Add<ISortTermParser, SortTermParser>(options.ServiceLifetime);
            services.Add<ISortingContext, SortingContext>(options.ServiceLifetime);

            services.Add<IStrainerPropertyMapper, StrainerPropertyMapper>(options.ServiceLifetime);

            services.Add<IStrainerCustomMethodsContext, StrainerCustomMethodsContext>(options.ServiceLifetime);

            services.Add<IStrainerContext, StrainerContext>(options.ServiceLifetime);

            services.Add<IStrainerProcessor, TProcessor>(options.ServiceLifetime);

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

        private static StrainerOptions GetStrainerOptions(this IServiceCollection services)
        {
            using (var provider = services.BuildServiceProvider())
            {
                return provider.GetRequiredService<IOptions<StrainerOptions>>().Value;
            }
        }
    }
}
