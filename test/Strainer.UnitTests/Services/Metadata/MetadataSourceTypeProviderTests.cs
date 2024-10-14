using Fluorite.Strainer.Services.Metadata;
using System.Reflection;

namespace Fluorite.Strainer.UnitTests.Services.Metadata;

public class MetadataSourceTypeProviderTests
{
    private readonly MetadataSourceTypeProvider _provider;

    public MetadataSourceTypeProviderTests()
    {
        _provider = new MetadataSourceTypeProvider();
    }

    [Fact]
    public void Should_Throw_ForNullAssemblies()
    {
        // Act
        Action act = () => _provider.GetSourceTypes(assemblies: null);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void Should_Return_OnlyClassesOrValueTypes()
    {
        // Arrange
        var types = new Type[]
        {
            typeof(IDisposable),
            typeof(Version),
            typeof(DateTime),
        };
        var assemblyMock = Substitute.For<Assembly>();
        assemblyMock
            .GetTypes()
            .Returns(types);
        assemblyMock
            .FullName
            .Returns("TestAssembly");
        var assemblies = new Assembly[]
        {
            assemblyMock,
        };

        // Act
        var result = _provider.GetSourceTypes(assemblies);

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().Contain(typeof(Version));
        result.Should().Contain(typeof(DateTime));
        result.Should().HaveCount(2);
    }

    [Fact]
    public void Should_Return_TypesNotFromTraceDataCollector()
    {
        // Arrange
        var assemblyMock = Substitute.For<Assembly>();
        assemblyMock
            .FullName
            .Returns("Microsoft.VisualStudio.TraceDataCollector");
        var assemblies = new Assembly[]
        {
            assemblyMock,
        };

        // Act
        var result = _provider.GetSourceTypes(assemblies);

        // Assert
        result.Should().NotBeNull();
        result.Should().BeEmpty();
    }
}
