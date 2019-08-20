using Fluorite.Strainer.Services;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests
{
    public abstract class StrainerFixtureBase : IClassFixture<StrainerFactory>
    {
        protected StrainerFixtureBase(StrainerFactory factory)
        {
            Factory = factory;
        }

        protected StrainerFactory Factory { get; }
    }
}
