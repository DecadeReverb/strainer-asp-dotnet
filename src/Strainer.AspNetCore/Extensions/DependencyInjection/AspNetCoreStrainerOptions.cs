using Fluorite.Strainer.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Fluorite.Extensions.DependencyInjection
{
    /// <summary>
    /// Represents enhanced set of options for used by Strainer within
    /// ASP.NET Core application.
    /// </summary>
    public class AspNetCoreStrainerOptions : StrainerOptions
    {
        /// <summary>
        /// Initializes new instance of <see cref="AspNetCoreStrainerOptions"/>
        /// class.
        /// </summary>
        public AspNetCoreStrainerOptions()
        {

        }

        /// <summary>
        /// Gets or sets the lifetime of Strainer services.
        /// <para/>
        /// Defaults to <see cref="ServiceLifetime.Scoped"/>.
        /// </summary>
        public ServiceLifetime ServiceLifetime { get; set; } = ServiceLifetime.Scoped;
    }
}
