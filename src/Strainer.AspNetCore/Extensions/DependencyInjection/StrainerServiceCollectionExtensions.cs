using Fluorite.Strainer.AspNetCore.Services;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Sorting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;

namespace Fluorite.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extensions for adding Strainer services to <see cref="IServiceCollection"/>.
    /// </summary>
    public static class StrainerServiceCollectionExtensions
    {
        /// <summary>
        /// The default service lifetime for Strainer services.
        /// </summary>
        public const ServiceLifetime DefaultServiceLifetime = ServiceLifetime.Scoped;

        /// <summary>
        /// Adds Strainer services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TProcessor">
        /// The type of Strainer processor used.
        /// </typeparam>
        /// <param name="services">
        /// Current instance of <see cref="IServiceCollection"/>.
        /// </param>
        /// <param name="serviceLifetime">
        /// The service lifetime for Strainer services.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IServiceCollection"/> with added
        /// Strainer services, so additional calls can be chained.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Another Strainer processor was already registered within the
        /// current <see cref="IServiceCollection"/>.
        /// </exception>
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
            services.Add<ISortingWayFormatter, DescendingPrefixSortingWayFormatter>(serviceLifetime);
            services.Add<ISortTermParser, SortTermParser>(serviceLifetime);
            services.Add<ISortingContext, SortingContext>(serviceLifetime);

            services.Add<ICustomFilterMethodMapper, CustomFilterMethodMapper>(serviceLifetime);
            services.Add<ICustomSortMethodMapper, CustomSortMethodMapper>(serviceLifetime);
            services.Add<ICustomMethodsContext, CustomMethodsContext>(serviceLifetime);

            services.Add<IPropertyMetadataProvider, PropertyMetadataMapper>(serviceLifetime);
            services.Add<IPropertyMetadataProvider, AttributeMetadataProvider>(serviceLifetime);
            services.Add<IMainMetadataProvider, MainMetadataProvider>(serviceLifetime);

            services.Add<IPropertyMetadataMapper, PropertyMetadataMapper>(serviceLifetime);
            services.Add<IStrainerContext, StrainerContext>(serviceLifetime);
            services.Add<IStrainerProcessor, TProcessor>(serviceLifetime);

            return new StrainerBuilder(services, serviceLifetime);
        }

        /// <summary>
        /// Adds Strainer services to the <see cref="IServiceCollection"/>
        /// with a configuration.
        /// </summary>
        /// <typeparam name="TProcessor">
        /// The type of Strainer processor used.
        /// </typeparam>
        /// <param name="services">
        /// Current instance of <see cref="IServiceCollection"/>.
        /// </param>
        /// <param name="configuration">
        /// A configuration used to bind against <see cref="StrainerOptions"/>.
        /// </param>
        /// <param name="serviceLifetime">
        /// The service lifetime for Strainer services.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IServiceCollection"/> with added
        /// Strainer services, so additional calls can be chained.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configuration"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Another Strainer processor was already registered within the
        /// current <see cref="IServiceCollection"/>.
        /// </exception>
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

            var builder = services.AddStrainer<TProcessor>(serviceLifetime);

            return builder;
        }

        /// <summary>
        /// Adds Strainer services to the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <typeparam name="TProcessor">
        /// The type of Strainer processor used.
        /// </typeparam>
        /// <param name="services">
        /// Current instance of <see cref="IServiceCollection"/>.
        /// </param>
        /// <param name="configure">
        /// An action used to configure <see cref="StrainerOptions"/>.
        /// </param>
        /// <param name="serviceLifetime">
        /// The service lifetime for Strainer services.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IServiceCollection"/> with added
        /// Strainer services, so additional calls can be chained.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="configure"/> is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        /// Another Strainer processor was already registered within the
        /// current <see cref="IServiceCollection"/>.
        /// </exception>
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

            var builder = services.AddStrainer<TProcessor>(serviceLifetime);

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
    }
}
