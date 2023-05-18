using Fluorite.Strainer.Services.Metadata;
using Moq;
using System.Reflection;

namespace Fluorite.Strainer.UnitTests.Services.Metadata
{
    public class AssemblySourceProviderTests
    {
        [Fact]
        public void Should_Return_CurrentAppDomainAssemblies()
        {
            // Arrange
            var assemblies = new Assembly[]
            {
                Mock.Of<Assembly>(),
            };
            var provider = new AssemblySourceProvider(assemblies); ;

            // Act
            var result = provider.GetAssemblies();

            // Assert
            result.Should().NotBeNullOrEmpty();
            result.Should().BeEquivalentTo(assemblies);
        }
    }
}
