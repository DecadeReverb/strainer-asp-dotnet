using FluentAssertions;
using Fluorite.Extensions.DependencyInjection;
using Fluorite.Strainer.Services.Metadata;
using Fluorite.Strainer.Services.Modules;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.UnitTests.AspNetCore
{
    public class FluentApiMetadataProviderTests
    {
        [Fact]
        public void FluentApiMetadataProvider_Works_When_Resolved_From_ServiceCollection()
        {
            // Arrange
            var services = new ServiceCollection();
            services.AddStrainer(new[] { typeof(TestModule) });
            var serviceProvider = services.BuildServiceProvider();
            var metadataProvidersWrapper = serviceProvider.GetRequiredService<IMetadataProvidersWrapper>();
            var fluentApiMetadataProvider = metadataProvidersWrapper
                .GetMetadataProviders()
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

        private class TestModule : StrainerModule
        {
            public override void Load()
            {
                base.Load();

                AddProperty<Post>(post => post.Id)
                    .IsFilterable()
                    .IsSortable()
                    .IsDefaultSort();
            }
        }
    }
}
