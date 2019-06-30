using Microsoft.Extensions.Options;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.IntegrationTests.Services
{
    public class StrainerOptionsAccessor : IOptions<StrainerOptions>
    {
        public StrainerOptions Value { get; }

        public StrainerOptionsAccessor()
        {
            Value = new StrainerOptions()
            {
                ThrowExceptions = true
            };
        }
    }
}
