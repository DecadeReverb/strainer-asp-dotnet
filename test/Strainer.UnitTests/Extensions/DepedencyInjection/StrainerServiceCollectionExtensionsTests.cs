using Fluorite.Extensions.DependencyInjection;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Filtering.Terms;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Configuration;
using Fluorite.Strainer.Services.Filtering;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Modules;
using Fluorite.Strainer.Services.Sorting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using System.Reflection;

namespace Fluorite.Strainer.UnitTests.Extensions.DepedencyInjection
{
    public class StrainerServiceCollectionExtensionsTests
    {
        [Fact]
        public void ExtensionMethod_AddsStrainer()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddStrainer();
            using var serviceProvider = services.BuildServiceProvider();
            var processor = serviceProvider.GetService<IStrainerProcessor>();

            // Assert
            processor.Should().NotBeNull();
        }

        [Fact]
        public void ExtensionMethod_AddsStrainer_WithAssemblies()
        {
            // Arrange
            var services = new ServiceCollection();
            var assemblyMock = new Mock<Assembly>();
            assemblyMock
                .Setup(x => x.GetTypes())
                .Returns(new[] { typeof(DerivedModule), typeof(BaseModule) });

            // Act
            services.AddStrainer(new[] { assemblyMock.Object });
            using var serviceProvider = services.BuildServiceProvider();
            var metadataFacade = serviceProvider.GetService<IMetadataFacade>();

            // Assert
            metadataFacade.Should().NotBeNull();
            metadataFacade
                .GetDefaultMetadata<Post>()
                .Should()
                .NotBeNull();
        }

        [Fact]
        public void ExtensionMethod_AddsStrainer_WithModuleTypes()
        {
            // Arrange
            var services = new ServiceCollection();
            var types = new[] { typeof(DerivedModule) };

            // Act
            services.AddStrainer(types);
            using var serviceProvider = services.BuildServiceProvider();
            var metadataFacade = serviceProvider.GetService<IMetadataFacade>();

            // Assert
            metadataFacade.Should().NotBeNull();
            metadataFacade
                .GetDefaultMetadata<Post>()
                .Should()
                .NotBeNull();
        }

        [Fact]
        public void ExtensionMethod_AddsStrainer_WithAction()
        {
            // Arrange
            var defaultPageSize = 20;
            var services = new ServiceCollection();

            // Act
            services.AddStrainer(options => options.DefaultPageSize = defaultPageSize);
            using var serviceProvider = services.BuildServiceProvider();
            var strainerOptionsProvider = serviceProvider.GetService<IStrainerOptionsProvider>();

            // Assert
            strainerOptionsProvider.Should().NotBeNull();
            strainerOptionsProvider.GetStrainerOptions().DefaultPageSize.Should().Be(defaultPageSize);
        }

        [Fact]
        public void ExtensionMethod_AddsStrainer_WithAction_AndAssemblies()
        {
            // Arrange
            var defaultPageSize = 20;
            var services = new ServiceCollection();

            var assemblyMock = new Mock<Assembly>();
            assemblyMock
                .Setup(x => x.GetTypes())
                .Returns(new[] { typeof(DerivedModule), typeof(BaseModule) });

            // Act
            services.AddStrainer(options => options.DefaultPageSize = defaultPageSize, new[] { assemblyMock.Object });
            using var serviceProvider = services.BuildServiceProvider();
            var metadataFacade = serviceProvider.GetService<IMetadataFacade>();
            var strainerOptionsProvider = serviceProvider.GetService<IStrainerOptionsProvider>();

            // Assert
            metadataFacade.Should().NotBeNull();
            metadataFacade
                .GetDefaultMetadata<Post>()
                .Should()
                .NotBeNull();

            strainerOptionsProvider.Should().NotBeNull();
            strainerOptionsProvider.GetStrainerOptions().DefaultPageSize.Should().Be(defaultPageSize);
        }

        [Fact]
        public void ExtensionMethod_AddsStrainer_WithConfiguration()
        {
            // Arrange
            var defaultPageSize = 20;
            var services = new ServiceCollection();
            var strainerOptions = new Dictionary<string, string>
            {
                { nameof(StrainerOptions.DefaultPageSize) , defaultPageSize.ToString() },
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(strainerOptions)
                .Build();

            // Act
            services.AddStrainer(configuration);
            using var serviceProvider = services.BuildServiceProvider();
            var strainerOptionsProvider = serviceProvider.GetService<IStrainerOptionsProvider>();

            // Assert
            strainerOptionsProvider.Should().NotBeNull();
            strainerOptionsProvider.GetStrainerOptions().DefaultPageSize.Should().Be(defaultPageSize);
        }

        [Fact]
        public void ExtensionMethod_AddsStrainer_WithConfiguration_AndAssemblies()
        {
            // Arrange
            var services = new ServiceCollection();
            var defaultPageSize = 20;
            var strainerOptions = new Dictionary<string, string>
            {
                { nameof(StrainerOptions.DefaultPageSize) , defaultPageSize.ToString() },
            };
            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(strainerOptions)
                .Build();

            var assemblyMock = new Mock<Assembly>();
            assemblyMock
                .Setup(x => x.GetTypes())
                .Returns(new[] { typeof(DerivedModule), typeof(BaseModule) });

            // Act
            services.AddStrainer(configuration, new[] { assemblyMock.Object });
            using var serviceProvider = services.BuildServiceProvider();
            var metadataFacade = serviceProvider.GetService<IMetadataFacade>();
            var strainerOptionsProvider = serviceProvider.GetService<IStrainerOptionsProvider>();


            // Assert
            metadataFacade.Should().NotBeNull();
            metadataFacade
                .GetDefaultMetadata<Post>()
                .Should()
                .NotBeNull();

            strainerOptionsProvider.Should().NotBeNull();
            strainerOptionsProvider.GetStrainerOptions().DefaultPageSize.Should().Be(defaultPageSize);
        }

        [Theory]
        [InlineData(ServiceLifetime.Transient)]
        [InlineData(ServiceLifetime.Scoped)]
        [InlineData(ServiceLifetime.Singleton)]
        public void ExtensionMethod_AddsStrainer_WithCustomServiceLifetime(ServiceLifetime serviceLifetime)
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddStrainer(serviceLifetime);

            // Assert
            services.FirstOrDefault(s => s.ServiceType == typeof(IStrainerProcessor))
                .Lifetime
                .Should()
                .Be(serviceLifetime);
        }

        [Fact]
        public void ExtensionMethod_AddsStrainer_WithStronglyTypedModule()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddStrainer(new[] { typeof(StronglyTypedStrainerModule) });
            using var serviceProvider = services.BuildServiceProvider();

            // Assert
            serviceProvider
                .GetRequiredService<IMetadataFacade>()
                .GetDefaultMetadata<Post>()
                .Should()
                .NotBeNull();
        }

        [Fact]
        public void ExtensionMethod_AddsStrainerConfiguration_From_Modules_Even_Not_Directly_Deriving_From_StrainerModule_Class()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddStrainer(new[] { typeof(DerivedModule) });
            using var serviceProvider = services.BuildServiceProvider();

            // Assert
            serviceProvider
                .GetRequiredService<IMetadataFacade>()
                .GetDefaultMetadata<Post>()
                .Should()
                .NotBeNull();
        }

        [Fact]
        public void ResolvingStrainer_Throws_When_Passed_Not_A_Strainer_Module_Type()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act & Assert
            services.AddStrainer(new[] { typeof(Exception) });
            using var serviceProvider = services.BuildServiceProvider();
            Assert.Throws<InvalidOperationException>(() => serviceProvider.GetRequiredService<IStrainerConfigurationProvider>());
        }

        [Fact]
        public void ResolvingStrainer_Throws_When_Passed_Abstract_Module_Type()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act & Assert
            services.AddStrainer(new[] { typeof(BaseModule) });
            using var serviceProvider = services.BuildServiceProvider();
            Assert.Throws<InvalidOperationException>(() => serviceProvider.GetRequiredService<IStrainerConfigurationProvider>());
        }

        [Fact]
        public void ExtensionMethod_Throws_WhenStrainerRegisteredSecondTime()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddStrainer();

            // Act
            Action act = () => services.AddStrainer();

            // Assert
            act.Should().ThrowExactly<InvalidOperationException>()
                .WithMessage("Unable to registrer Strainer services because they have been registered already.");
        }

        [Fact]
        public void ExtensionMethod_AddsStrainer_With_TwoPropertyMetadataProviders()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddStrainer();
            using var serviceProvider = services.BuildServiceProvider();
            var propertyMetadataProviders = serviceProvider.GetService<IEnumerable<IMetadataProvider>>();

            // Assert
            propertyMetadataProviders.Should().NotBeNullOrEmpty();
            propertyMetadataProviders.Should().HaveCount(2);
        }

        [Fact]
        public void ExtensionMethod_AddsStrainer_WithCustomService_FilterTermParser()
        {
            // Arrange
            var services = new ServiceCollection();

            // Act
            services.AddStrainer();
            services.AddScoped<IFilterTermParser, TestFilterTermParser>();
            using var serviceProvider = services.BuildServiceProvider();
            var postExtensionFilterTermParser = serviceProvider.GetRequiredService<IFilterTermParser>();

            // Assert
            postExtensionFilterTermParser
                .Should()
                .BeAssignableTo<TestFilterTermParser>(
                    "Because DI container should return service that was registered last.");
        }

        [Fact]
        public void CustomSortingWayFormatter_Works_When_AddedToServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddStrainer();
            services.AddScoped<ISortingWayFormatter, TestSortingWayFormatter>();
            using var serviceProvider = services.BuildServiceProvider();

            // Act
            var formatter = serviceProvider.GetRequiredService<ISortingWayFormatter>();

            // Assert
            formatter
                .Should()
                .BeAssignableTo<TestSortingWayFormatter>();
        }

        private abstract class BaseModule : StrainerModule
        {

        }

        private class DerivedModule : BaseModule
        {
            public override void Load(IStrainerModuleBuilder builder)
            {
                builder.AddObject<Post>(p => p.Title);
            }
        }

        private class StronglyTypedStrainerModule : StrainerModule<Post>
        {
            public override void Load(IStrainerModuleBuilder<Post> builder)
            {
                builder
                    .AddObject(post => post.Title)
                    .IsFilterable()
                    .IsSortable();
            }
        }

        private class Post
        {
            public string Title { get; set; }
        }

        private class TestFilterTermParser : IFilterTermParser
        {
            public IList<IFilterTerm> GetParsedTerms(string input)
            {
                throw new NotImplementedException();
            }
        }

        private class TestSortingWayFormatter : ISortingWayFormatter
        {
            public string Format(string input, SortingWay sortingWay)
            {
                throw new NotImplementedException();
            }

            public SortingWay GetSortingWay(string input)
            {
                throw new NotImplementedException();
            }

            public string Unformat(string input, SortingWay sortingWay)
            {
                throw new NotImplementedException();
            }
        }
    }
}
