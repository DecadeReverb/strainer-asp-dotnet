using Fluorite.Strainer.Models.Configuration;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services.Configuration;

namespace Fluorite.Strainer.UnitTests.Services.Configuration;

public class ConfigurationCustomMethodsProviderTests
{
    private readonly IStrainerConfigurationProvider _strainerConfigurationProviderMock = Substitute.For<IStrainerConfigurationProvider>();

    private readonly ConfigurationCustomMethodsProvider _provider;

    public ConfigurationCustomMethodsProviderTests()
    {
        _provider = new ConfigurationCustomMethodsProvider(_strainerConfigurationProviderMock);
    }

    [Fact]
    public void Should_Throw_ForNullConfigurationProvider()
    {
        // Act
        Action act = () => _ = new ConfigurationCustomMethodsProvider(strainerConfigurationProvider: null);

        // Assert
        act.Should().ThrowExactly<ArgumentNullException>();
    }

    [Fact]
    public void Should_Return_CustomFilterMethods()
    {
        // Arrange
        var customFilterMethods = new Dictionary<Type, IReadOnlyDictionary<string, ICustomFilterMethod>>
        {
            {
                typeof(object),
                new Dictionary<string, ICustomFilterMethod>
                {
                    {
                        "foo",
                        Substitute.For<ICustomFilterMethod>()
                    },
                }
            },
        };

        var strainerConfigurationMock = Substitute.For<IStrainerConfiguration>();
        strainerConfigurationMock
            .CustomFilterMethods
            .Returns(customFilterMethods);

        _strainerConfigurationProviderMock
            .GetStrainerConfiguration()
            .Returns(strainerConfigurationMock);

        // Act
        var result = _provider.GetCustomFilterMethods();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(customFilterMethods);
    }

    [Fact]
    public void Should_Return_CustomSortMethods()
    {
        // Arrange
        var customSortMethods = new Dictionary<Type, IReadOnlyDictionary<string, ICustomSortMethod>>
        {
            {
                typeof(object),
                new Dictionary<string, ICustomSortMethod>
                {
                    {
                        "foo",
                        Substitute.For<ICustomSortMethod>()
                    },
                }
            },
        };

        var strainerConfigurationMock = Substitute.For<IStrainerConfiguration>();
        strainerConfigurationMock
            .CustomSortMethods
            .Returns(customSortMethods);

        _strainerConfigurationProviderMock
            .GetStrainerConfiguration()
            .Returns(strainerConfigurationMock);

        // Act
        var result = _provider.GetCustomSortMethods();

        // Assert
        result.Should().NotBeNull();
        result.Should().BeSameAs(customSortMethods);
    }
}
