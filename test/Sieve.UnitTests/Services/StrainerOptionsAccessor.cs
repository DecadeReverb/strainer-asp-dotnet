using Microsoft.Extensions.Options;
using Strainer.Models;

namespace Strainer.UnitTests
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
