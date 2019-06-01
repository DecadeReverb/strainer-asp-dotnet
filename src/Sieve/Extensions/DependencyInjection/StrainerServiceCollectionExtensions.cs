using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Strainer.Models;
using Strainer.Services;
using Strainer.Services.Filtering;
using Strainer.Services.Sorting;

namespace Strainer.Extensions.DependencyInjection
{
    public static class StrainerServiceCollectionExtensions
    {
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

            using (var provider = services.BuildServiceProvider())
            {
                // Add Strainer options only if they weren't configured yet.
                if (!services.Any(d => d.ServiceType == typeof(IOptions<StrainerOptions>)))
                {
                    var configuration = provider.GetRequiredService<IConfiguration>();
                    services.Configure<StrainerOptions>(configuration.GetSection("Strainer"));
                }
            }

            services.AddScoped<IFilterOperatorParser, FilterOperatorParser>();
            services.AddScoped<IFilterOperatorProvider, FilterOperatorProvider>();
            services.AddScoped<IFilterOperatorValidator, FilterOperatorValidator>();
            services.AddScoped<IFilterTermParser, FilterTermParser>();
            services.AddScoped<IFilteringContext, FilteringContext>();

            services.AddScoped<ISortTermParser, SortTermParser>();
            services.AddScoped<ISortingContext, SortingContext>();

            services.AddScoped<IStrainerPropertyMapper, StrainerPropertyMapper>();

            services.AddScoped<IStrainerCustomMethodsContext, StrainerCustomMethodsContext>();

            services.AddScoped<IStrainerContext, StrainerContext>();

            services.AddScoped<IStrainerProcessor, TProcessor>();

            return new StrainerBuilder(services);
        }

        public static IStrainerBuilder AddCustomFilterMethods<TFilterMethods>(this IStrainerBuilder builder)
            where TFilterMethods : class, IStrainerCustomFilterMethods
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddScoped<IStrainerCustomFilterMethods, TFilterMethods>();

            return builder;
        }

        public static IStrainerBuilder AddCustomSortMethods<TSortMethods>(this IStrainerBuilder builder)
            where TSortMethods : class, IStrainerCustomSortMethods
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddScoped<IStrainerCustomSortMethods, TSortMethods>();

            return builder;
        }
    }
}
