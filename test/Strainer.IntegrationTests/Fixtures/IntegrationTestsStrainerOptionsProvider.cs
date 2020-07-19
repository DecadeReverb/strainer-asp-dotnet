using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;

namespace Fluorite.Strainer.IntegrationTests.Fixtures
{
    public class IntegrationTestsStrainerOptionsProvider : IStrainerOptionsProvider
    {
        private readonly StrainerOptions _strainerOptions;

        public IntegrationTestsStrainerOptionsProvider()
        {
            _strainerOptions = new StrainerOptions();
        }

        public StrainerOptions GetStrainerOptions() => _strainerOptions;
    }
}
