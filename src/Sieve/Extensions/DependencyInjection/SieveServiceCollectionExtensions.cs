using System;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using Sieve.Services.Filtering;

namespace Sieve.Extensions.DependencyInjection
{
    public static class SieveServiceCollectionExtensions
    {
        public static ISieveBuilder AddSieve<TProcessor>(this IServiceCollection services, Action<SieveOptions> options)
            where TProcessor : class, ISieveProcessor
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            if (options == null)
            {
                throw new ArgumentNullException(nameof(options));
            }

            var builder = services.AddSieve<TProcessor>();
            builder.Services.PostConfigure(options);

            return builder;
        }

        public static ISieveBuilder AddSieve<TProcessor>(this IServiceCollection services)
            where TProcessor : class, ISieveProcessor
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            using (var provider = services.BuildServiceProvider())
            {
                // Add Sieve options only if they weren't configured yet.
                if (!services.Any(d => d.ServiceType == typeof(IOptions<SieveOptions>)))
                {
                    var configuration = provider.GetRequiredService<IConfiguration>();
                    services.Configure<SieveOptions>(configuration.GetSection("Sieve"));
                }
            }

            services.AddScoped<IFilterOperatorParser, FilterOperatorParser>();
            services.AddScoped<IFilterOperatorProvider, FilterOperatorProvider>();
            services.AddScoped<IFilterOperatorValidator, FilterOperatorValidator>();
            services.AddScoped<IFilterOperatorContext, FilterOperatorContext>();

            services.AddScoped<IFilterTermParser, FilterTermParser>();
            services.AddScoped<IFilterTermContext, FilterTermContext>();

            services.AddScoped<ISievePropertyMapper, SievePropertyMapper>();

            services.AddScoped<ISieveCustomMethodsContext, SieveCustomMethodsContext>();

            services.AddScoped<ISieveContext, SieveContext>();

            services.AddScoped<ISieveProcessor, TProcessor>();

            return new SieveBuilder(services);
        }

        public static ISieveBuilder AddCustomFilterMethods<TFilterMethods>(this ISieveBuilder builder)
            where TFilterMethods : class, ISieveCustomFilterMethods
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddScoped<ISieveCustomFilterMethods, TFilterMethods>();

            return builder;
        }

        public static ISieveBuilder AddCustomSortMethods<TSortMethods>(this ISieveBuilder builder)
            where TSortMethods : class, ISieveCustomSortMethods
        {
            if (builder == null)
            {
                throw new ArgumentNullException(nameof(builder));
            }

            builder.Services.AddScoped<ISieveCustomSortMethods, TSortMethods>();

            return builder;
        }
    }
}
