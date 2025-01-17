﻿using Fluorite.Strainer.Services.Metadata;

namespace Fluorite.Strainer.UnitTests.Services.Metadata;

public class AppDomainAssemblySourceProviderTests
{
    private readonly AppDomainAssemblySourceProvider _provider;

    public AppDomainAssemblySourceProviderTests()
    {
        _provider = new AppDomainAssemblySourceProvider();
    }

    [Fact]
    public void Should_Return_CurrentAppDomainAssemblies()
    {
        // Arrange
        var assemblies = AppDomain.CurrentDomain.GetAssemblies();

        // Act
        var result = _provider.GetAssemblies();

        // Assert
        result.Should().NotBeNullOrEmpty();
        result.Should().BeEquivalentTo(assemblies);
    }
}
