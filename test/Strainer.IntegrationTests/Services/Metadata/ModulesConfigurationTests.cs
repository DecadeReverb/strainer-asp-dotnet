using FluentAssertions;
using Fluorite.Extensions.DependencyInjection;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Modules;
using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Services.Metadata
{
    public class ModulesConfigurationTests
    {
        [Fact]
        public void ModuleConfiguration_Works_For_Adding_Property()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddStrainer(new[] { typeof(PropertyTestModule) });
            var serviceProvider = services.BuildServiceProvider();
            var metadataProviders = serviceProvider.GetRequiredService<IEnumerable<IMetadataProvider>>();
            var fluentApiMetadataProvider = metadataProviders
                .OfType<FluentApiMetadataProvider>()
                .Single();

            // Act
            var metadatas = fluentApiMetadataProvider.GetPropertyMetadatas<Post>();

            // Assert
            metadatas.Should().NotBeNullOrEmpty();
            metadatas.Should().HaveSameCount(typeof(Post).GetProperties());
            metadatas.First().Name.Should().Be(nameof(Post.Id));
        }

        private class Post
        {
            public int Id { get; set; }
        }

        private class PropertyTestModule : StrainerModule
        {
            public override void Load()
            {
                AddProperty<Post>(post => post.Id)
                    .IsFilterable()
                    .IsSortable()
                    .IsDefaultSort();
            }
        }
    }
}
