﻿using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Microsoft.Extensions.Options;
using System;

namespace Fluorite.Strainer.AspNetCore.Services
{
    /// <summary>
    /// Provides access to <see cref="StrainerOptions"/> when using Strainer
    /// within ASP.NET Core applications registered as singleton services.
    /// </summary>
    public class AspNetCoreSingletonStrainerOptionsProvider : IStrainerOptionsProvider
    {
        private readonly IOptions<StrainerOptions> _options;

        /// <summary>
        /// Initializes new instance of <see cref="AspNetCoreStrainerOptionsProvider"/>
        /// class.
        /// </summary>
        /// <param name="options">
        /// The Strainer options wrapper in <see cref="IOptions{TOptions}"/>.
        /// </param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="options"/> is <see langword="null"/>.
        /// </exception>
        public AspNetCoreSingletonStrainerOptionsProvider(IOptions<StrainerOptions> options)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Gets the <see cref="StrainerOptions"/>.
        /// </summary>
        /// <returns>
        /// An instance of <see cref="StrainerOptions"/>.
        /// </returns>
        public StrainerOptions GetStrainerOptions() => _options.Value;
    }
}