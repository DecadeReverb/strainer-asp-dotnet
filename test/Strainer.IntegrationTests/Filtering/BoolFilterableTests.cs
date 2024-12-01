using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.IntegrationTests.Filtering;

public class BoolFilterableTests : StrainerFixtureBase
{
    public BoolFilterableTests(StrainerFactory factory) : base(factory)
    {

    }

    [Fact]
    public void FilteringForBoolValues_Works()
    {
        // Arrange
        var posts = new Post[]
        {
            new()
            {
                IsDraft = true,
            },
            new()
            {
                IsDraft = false,
            },
        }.AsQueryable();
        var model = new StrainerModel()
        {
            Filters = "IsDraft==false",
        };
        var processor = Factory.CreateDefaultProcessor();

        // Act
        var result = processor.ApplyFiltering(model, posts);

        // Assert
        result.Should().Contain(p => !p.IsDraft);
    }

    private class Post
    {
        [StrainerProperty]
        public bool IsDraft { get; set; }
    }
}
