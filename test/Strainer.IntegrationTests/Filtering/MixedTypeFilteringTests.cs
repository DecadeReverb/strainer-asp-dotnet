using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.IntegrationTests.Filtering;

public class MixedTypeFilteringTests : StrainerFixtureBase
{
    public MixedTypeFilteringTests(StrainerFactory factory) : base(factory)
    {
    }

    [Fact]
    public void Should_Throw_WhenFilteringUsingWrongValueType()
    {
        // Arrange
        var processor = Factory.CreateDefaultProcessor<PostStrainerModule>(options => options.ThrowExceptions = true);
        var source = new List<Post>
        {
            new () { IsActive = true },
        }.AsQueryable();
        var model = new StrainerModel
        {
            Filters = $"{nameof(Post.IsActive)}==303",
        };

        // Act
        Action act = () => processor.Apply(model, source);

        // Assert
        act.Should().ThrowExactly<StrainerConversionException>()
            .WithMessage("Failed to convert value '303' to type 'System.Boolean'.");
    }

    private class Post
    {
        public bool IsActive { get; set; }
    }

    private class PostStrainerModule : StrainerModule<Post>
    {
        public override void Load(IStrainerModuleBuilder<Post> builder)
        {
            builder
                .AddProperty(x => x.IsActive)
                .IsFilterable()
                .IsSortable()
                .IsDefaultSort();
        }
    }
}
