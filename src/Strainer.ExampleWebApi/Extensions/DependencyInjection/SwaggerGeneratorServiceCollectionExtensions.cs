using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Linq;

namespace Fluorite.Extensions.DependencyInjection
{
    /// <summary>
    /// Provides extension methods for adding Swagger generator to
    /// <see cref="IServiceCollection"/>.
    /// </summary>
    public static class SwaggerGeneratorServiceCollectionExtensions
    {
        /// <summary>
        /// Adds Swagger generator to the <see cref="IServiceCollection"/>
        /// with default options registering one main Swagger document.
        /// </summary>
        /// <param name="services">
        /// Current <see cref="IServiceCollection"/> instance.
        /// </param>
        /// <returns>
        /// An instance of <see cref="IServiceCollection"/> with added
        /// Swagger generator so additional calls can be chained.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> is <see langword="null"/>.
        /// </exception>
        public static IServiceCollection AddSwaggerGenWithDefaultOptions(this IServiceCollection services)
        {
            if (services == null)
            {
                throw new ArgumentNullException(nameof(services));
            }

            return services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "Strainer example API",
                    Description = "Example API incorporating most Strainer concepts.",
                    License = new OpenApiLicense
                    {
                        Name = "Apache License 2.0",
                        Url = new Uri("https://gitlab.com/fluorite/strainer/blob/master/LICENSE"),
                    }
                });

                // Set the comments path for the Swagger JSON and UI.
                var xmlFiles = Directory.GetFiles(AppContext.BaseDirectory, "Fluorite*.xml", SearchOption.TopDirectoryOnly).ToList();
                xmlFiles.ForEach(xmlFile => options.IncludeXmlComments(xmlFile));
            });
        }
    }
}
