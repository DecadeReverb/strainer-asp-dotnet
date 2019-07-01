using Microsoft.Extensions.DependencyInjection;
using System;

namespace Fluorite.Extensions.DependencyInjection
{
    /// <summary>
    /// Represents builder for configuring Strainer services to
    /// <see cref="IServiceCollection"/>.
    /// </summary>
    public class StrainerBuilder : IStrainerBuilder
    {
        /// <summary>
        /// Initializes new instance of <see cref="StrainerBuilder"/> class.
        /// </summary>
        /// <param name="services">
        /// The service collection.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="services"/> is <see langword="null"/>.
        /// </exception>
        public StrainerBuilder(IServiceCollection services)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
        }

        /// <summary>
        /// Gets the services collection.
        /// </summary>
        public IServiceCollection Services { get; }
    }
}
