using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sieve.Models;
using Sieve.Services;

namespace Sieve.Extensions.DependencyInjection
{
    public static class SieveServiceCollectionExtensions
    {
        public static ISieveBuilder AddSieve<TProcessor>(this IServiceCollection services)
            where TProcessor : class, ISieveProcessor
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            IConfiguration configuration = null;
            using (var provider = services.BuildServiceProvider())
            {
                configuration = provider.GetRequiredService<IConfiguration>();
            }

            services.Configure<SieveOptions>(configuration.GetSection("Sieve"));

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
