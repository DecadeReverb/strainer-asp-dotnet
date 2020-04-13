using Fluorite.Strainer.Models.Configuration;

namespace Fluorite.Strainer.Services.Configuration
{
    public interface IStrainerConfigurationProvider
    {
        /// <summary>
        /// Gets the Stariner configuration.
        /// </summary>
        IStrainerConfiguration GetStrainerConfiguration();
    }
}
