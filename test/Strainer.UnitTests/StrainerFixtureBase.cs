using Xunit;

namespace Fluorite.Strainer.UnitTests
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
