using FluentAssertions;
using Fluorite.Extensions.DependencyInjection;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering.Terms;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Sorting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
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

        [Fact]
        public void ExtensionMethod_AddsStrainerWithCustomOptions()
        {
            // Arrange
            var defaultPageSize = 20;
            IConfiguration configuration = new ConfigurationRoot(new List<IConfigurationProvider>());
            var services = new ServiceCollection().AddSingleton(configuration);

            // Act
            var serviceProvider = services.BuildServiceProvider();
            services.AddStrainer<StrainerProcessor>(options =>
            {
                options.DefaultPageSize = defaultPageSize;
            });
            serviceProvider = services.BuildServiceProvider();
            var postExtensionStrainer = serviceProvider.GetService<IStrainerProcessor>();
            var postExtensionStrainerOptions = serviceProvider.GetService<IOptions<StrainerOptions>>().Value;

            // Assert
            postExtensionStrainer
                .Should()
                .NotBeNull("Because extension method should add " +
                        "Strainer to the service collection.");
            postExtensionStrainerOptions
                .DefaultPageSize
                .Should()
                .Be(defaultPageSize);
        }

        [Fact]
        public void ExtensionMethod_AddsStrainerWithCustomService_FilterTermParser()
        {
            // Arrange
            IConfiguration configuration = new ConfigurationRoot(new List<IConfigurationProvider>());
            var services = new ServiceCollection().AddSingleton(configuration);

            // Act
            services.AddStrainer<StrainerProcessor>();
            services.AddScoped<IFilterTermParser, _TestFilterTermParser>();
            var serviceProvider = services.BuildServiceProvider();
            var postExtensionStrainer = serviceProvider.GetService<IStrainerProcessor>();
            var postExtensionFilterTermParser = serviceProvider.GetService<IFilterTermParser>();

            // Assert
            postExtensionStrainer
                .Should()
                .NotBeNull("Because extension method should add " +
                        "Strainer to the service collection.");
            postExtensionFilterTermParser
                .Should()
                .BeAssignableTo<_TestFilterTermParser>(
                    "Because DI container should return service that was registered last.");
        }
    }

    internal class _TestFilterTermParser : IFilterTermParser
    {
        public IList<IFilterTerm> GetParsedTerms(string input)
        {
            throw new NotImplementedException();
        }
    }
}
