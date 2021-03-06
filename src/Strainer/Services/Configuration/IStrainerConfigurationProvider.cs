using Fluorite.Strainer.Models.Configuration;

namespace Fluorite.Strainer.Services.Configuration
{
    /// <summary>
    /// Provides read-only <see cref="IStrainerConfiguration"/>.
    /// </summary>
    public interface IStrainerConfigurationProvider
    {
        /// <summary>
        /// Gets the Stariner configuration.
        /// </summary>
        IStrainerConfiguration GetStrainerConfiguration();
    }
}
