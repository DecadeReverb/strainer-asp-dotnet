using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using System;

namespace Fluorite.Strainer.IntegrationTests
{
    public class IntegrationTestsStrainerOptionsProvider : IStrainerOptionsProvider
    {
        private StrainerOptions _strainerOptions;

        public IntegrationTestsStrainerOptionsProvider()
        {
            _strainerOptions = new StrainerOptions();
        }

        public StrainerOptions GetStrainerOptions() => _strainerOptions;

        public void SetStrainerOptions(StrainerOptions strainerOptions)
        {
            if (strainerOptions == null)
            {
                throw new ArgumentNullException(nameof(strainerOptions));
            }

            _strainerOptions = strainerOptions;
        }
    }
}
