using Microsoft.Extensions.Options;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.UnitTests
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
