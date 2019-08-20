using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Microsoft.Extensions.Options;
using System;

namespace Fluorite.Strainer.AspNetCore.Services
{
    /// <summary>
    /// Provides access to <see cref="StrainerOptions"/> when using Strainer
    /// within ASP.NET Core applications.
    /// </summary>
    public class AspNetCoreStrainerOptionsProvider : IStrainerOptionsProvider
    {
        private readonly IOptionsSnapshot<StrainerOptions> options;

        /// <summary>
        /// Initializes new instance of <see cref="AspNetCoreStrainerOptionsProvider"/>
        /// class.
        /// </summary>
        /// <param name="options">
        /// The Strainer options wrapper in <see cref="IOptionsSnapshot{TOptions}"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="options"/> is <see langword="null"/>.
        /// </exception>
        public AspNetCoreStrainerOptionsProvider(IOptionsSnapshot<StrainerOptions> options)
        {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Gets <see cref="StrainerOptions"/>.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="StrainerOptions"/>.
        /// </returns>
        public StrainerOptions GetStrainerOptions() => options.Value;
    }
}
