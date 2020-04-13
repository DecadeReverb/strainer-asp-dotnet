using Fluorite.Strainer.Models.Configuration;
using System;

namespace Fluorite.Strainer.Services.Configuration
{
    public class StrainerConfigurationProvider : IStrainerConfigurationProvider
    {
        private readonly IStrainerConfiguration _strainerConfiguration;

        public StrainerConfigurationProvider(IStrainerConfiguration strainerConfiguration)
        {
            _strainerConfiguration = strainerConfiguration ?? throw new ArgumentNullException(nameof(strainerConfiguration));
        }

        public IStrainerConfiguration GetStrainerConfiguration() => _strainerConfiguration;
    }
}
