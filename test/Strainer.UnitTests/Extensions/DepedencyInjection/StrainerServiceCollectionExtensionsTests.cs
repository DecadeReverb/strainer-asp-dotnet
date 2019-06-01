using System.Collections.Generic;
using FluentAssertions;
using Fluorite.Extensions.DependencyInjection;
using Fluorite.Strainer.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Extensions.DepedencyInjection
{
    public class StrainerServiceCollectionExtensionsTests
    {
        [Fact]
        public void ExtensionMethod_AddsStrainer()
        {
            // Arrange
            IConfiguration configuration = new ConfigurationRoot(new List<IConfigurationProvider>());
            var services = new ServiceCollection().AddSingleton(configuration);

            // Act
            var serviceProvider = services.BuildServiceProvider();
            var preExtensionStrainer = serviceProvider.GetService<IStrainerProcessor>();
            services.AddStrainer<StrainerProcessor>();
            serviceProvider = services.BuildServiceProvider();
            var postExtensionStrainer = serviceProvider.GetService<IStrainerProcessor>();

            // Assert
            preExtensionStrainer
                .Should()
                .BeNull("Because Strainer has not been registered yet.");
            postExtensionStrainer
                .Should()
                .NotBeNull("Because extension method should add " +
                        "Strainer to the service collection.");
        }
    }
}
