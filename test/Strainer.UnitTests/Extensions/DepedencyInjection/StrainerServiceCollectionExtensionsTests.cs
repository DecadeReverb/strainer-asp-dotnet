using System.Collections.Generic;
using FluentAssertions;
using Fluorite.Extensions.DependencyInjection;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.UnitTests.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Extensions.DepedencyInjection
{
    public class StrainerServiceCollectionExtensionsTests
    {
        [Fact]
        public void ExtensionMethod_ShouldAddStrainer()
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

        [Fact]
        public void ExtensionMethod_ShouldAddStrainerWhileConfiguringOptions()
        {
            // Arrange
            var defaultPageSize = 20;
            IConfiguration configuration = new ConfigurationRoot(new List<IConfigurationProvider>());
            var services = new ServiceCollection().AddSingleton(configuration);

            // Act
            var serviceProvider = services.BuildServiceProvider();
            var preExtensionStrainer = serviceProvider.GetService<IStrainerProcessor>();
            services.AddStrainer<StrainerProcessor>(options =>
            {
                options.DefaultPageSize = defaultPageSize;
            });
            serviceProvider = services.BuildServiceProvider();
            var postExtensionStrainer = serviceProvider.GetService<IStrainerProcessor>();
            var postExtensionStrainerOptions = serviceProvider.GetService<IOptions<StrainerOptions>>()?.Value;

            // Assert
            preExtensionStrainer
                .Should()
                .BeNull("Because Strainer has not been registered yet.");
            postExtensionStrainer
                .Should()
                .NotBeNull("Because extension method should add " +
                        "Strainer to the service collection.");
            postExtensionStrainerOptions
                ?.DefaultPageSize
                .Should()
                .Be(defaultPageSize);
        }

        [Fact]
        public void ExtensionMethod_ShouldAddCustomFilterMethods()
        {
            // Arrange
            IConfiguration configuration = new ConfigurationRoot(new List<IConfigurationProvider>());
            var services = new ServiceCollection().AddSingleton(configuration);

            // Act
            var serviceProvider = services.BuildServiceProvider();
            var preExtensionStrainerCustomFilterMethods = serviceProvider.GetService<IStrainerCustomFilterMethods>();
            services.AddStrainer<StrainerProcessor>()
                .AddCustomFilterMethods<StrainerCustomFilterMethods>();
            serviceProvider = services.BuildServiceProvider();
            var postExtensionStrainerCustomFilterMethods = serviceProvider.GetService<IStrainerCustomFilterMethods>();

            // Assert
            preExtensionStrainerCustomFilterMethods
                .Should()
                .BeNull("Because Strainer has not been registered yet.");
            postExtensionStrainerCustomFilterMethods
                .Should()
                .NotBeNull("Because extension method should add Stariner " +
                        "custom filter methods to the service collection.");
        }

        [Fact]
        public void ExtensionMethod_ShouldAddCustomSortMethods()
        {
            // Arrange
            IConfiguration configuration = new ConfigurationRoot(new List<IConfigurationProvider>());
            var services = new ServiceCollection().AddSingleton(configuration);

            // Act
            var serviceProvider = services.BuildServiceProvider();
            var preExtensionStrainerCustomSortMethods = serviceProvider.GetService<IStrainerCustomSortMethods>();
            services.AddStrainer<StrainerProcessor>()
                .AddCustomSortMethods<StrainerCustomSortMethods>();
            serviceProvider = services.BuildServiceProvider();
            var postExtensionStrainerCustomSortMethods = serviceProvider.GetService<IStrainerCustomSortMethods>();

            // Assert
            preExtensionStrainerCustomSortMethods
                .Should()
                .BeNull("Because Strainer has not been registered yet.");
            postExtensionStrainerCustomSortMethods
                .Should()
                .NotBeNull("Because extension method should add Stariner " +
                        "custom sort methods to the service collection.");
        }
    }
}
