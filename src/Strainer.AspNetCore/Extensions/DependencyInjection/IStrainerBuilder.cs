using Microsoft.Extensions.DependencyInjection;

namespace Fluorite.Extensions.DependencyInjection
{
    /// <summary>
    /// Represents builder for configuring Strainer services to
    /// <see cref="IServiceCollection"/>.
    /// </summary>
    public interface IStrainerBuilder
    {
        /// <summary>
        /// Gets the services collection.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
