using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.Services
{
    /// <summary>
    /// Provides access to <see cref="StrainerOptions"/>.
    /// </summary>
    public interface IStrainerOptionsProvider
    {
        /// <summary>
        /// Gets <see cref="StrainerOptions"/>.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="StrainerOptions"/>.
        /// </returns>
        StrainerOptions GetStrainerOptions();
    }
}
