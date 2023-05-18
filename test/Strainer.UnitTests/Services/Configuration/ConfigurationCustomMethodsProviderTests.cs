using Fluorite.Strainer.Models.Configuration;
using Fluorite.Strainer.Models.Filtering;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services.Configuration;
using Moq;

namespace Fluorite.Strainer.UnitTests.Services.Configuration
{
    public class ConfigurationCustomMethodsProviderTests
    {
        private readonly Mock<IStrainerConfigurationProvider> _strainerConfigurationProviderMock = new();

        private readonly ConfigurationCustomMethodsProvider _provider;

        public ConfigurationCustomMethodsProviderTests()
        {
            _provider = new ConfigurationCustomMethodsProvider(_strainerConfigurationProviderMock.Object);
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
                            Mock.Of<ICustomFilterMethod>()
                        }
                    }
                }
            };

            var strainerConfigurationMock = new Mock<IStrainerConfiguration>();
            strainerConfigurationMock
                .SetupGet(x => x.CustomFilterMethods)
                .Returns(customFilterMethods);

            _strainerConfigurationProviderMock
                .Setup(x => x.GetStrainerConfiguration())
                .Returns(strainerConfigurationMock.Object);

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
                            Mock.Of<ICustomSortMethod>()
                        }
                    }
                }
            };

            var strainerConfigurationMock = new Mock<IStrainerConfiguration>();
            strainerConfigurationMock
                .SetupGet(x => x.CustomSortMethods)
                .Returns(customSortMethods);

            _strainerConfigurationProviderMock
                .Setup(x => x.GetStrainerConfiguration())
                .Returns(strainerConfigurationMock.Object);

            // Act
            var result = _provider.GetCustomSortMethods();

            // Assert
            result.Should().NotBeNull();
            result.Should().BeSameAs(customSortMethods);
        }
    }
}
