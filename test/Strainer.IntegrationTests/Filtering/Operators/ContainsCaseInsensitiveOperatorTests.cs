using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.IntegrationTests.Filtering.Operators;

public class ContainsCaseInsensitiveOperatorTests : StrainerFixtureBase
{
    public ContainsCaseInsensitiveOperatorTests(StrainerFactory factory) : base(factory)
    {

    }

    [Fact]
    public void ContainsCanBeCaseInsensitive()
    {
        // Arrange
        var queryable = new List<Post>
        {
            new()
            {
                Title = "Nice rock album.",
            },
            new()
            {
                Title = "A long time ago.",
            },
            new()
            {
                Title = "The end.",
            },
        }.AsQueryable();
        var model = new StrainerModel()
        {
            Filters = "Title@=*a",
        };
        var processor = Factory.CreateDefaultProcessor();

        // Act
        var result = processor.Apply(model, queryable);

        // Assert
        result.Should().OnlyContain(p => p.Title.Contains("a", StringComparison.OrdinalIgnoreCase));
    }

    private class Post
    {
        [StrainerProperty]
        public string Title { get; set; }
    }
}
