using Microsoft.Extensions.Options;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.UnitTests.Services
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
