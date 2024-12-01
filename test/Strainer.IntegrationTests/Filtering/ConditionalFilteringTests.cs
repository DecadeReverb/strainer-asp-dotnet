using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.IntegrationTests.Filtering;

public class ConditionalFilteringTests : StrainerFixtureBase
{
    public ConditionalFilteringTests(StrainerFactory factory) : base(factory)
    {

    }

    [Fact]
    public void OrValueFiltering_Works()
    {
        // Arrange
        var posts = new Post[]
        {
            new()
            {
                Title = "How not to die?",
            },
            new()
            {
                Title = "Why Java programmers can't C#",
            },
            new()
            {
                Title = "When the dinner will be ready?",
            },
        }.AsQueryable();
        var model = new StrainerModel()
        {
            Filters = "Title_=How|Why",
        };
        var processor = Factory.CreateDefaultProcessor();

        // Act
        var result = processor.Apply(model, posts);

        // Assert
        result.Should().OnlyContain(p => p.Title.StartsWith("How") || p.Title.StartsWith("Why"));
    }

    [Fact]
    public void OrValueFiltering_WithBraces_Work()
    {
        // Arrange
        var posts = new Post[]
        {
            new()
            {
                Title = ":)",
            },
            new()
            {
                Title = "(YES/NO)",
            },
            new()
            {
                Title = string.Empty,
            },
        }.AsQueryable();
        var model = new StrainerModel()
        {
            Filters = "Title@=(|)",
        };
        var processor = Factory.CreateDefaultProcessor();

        // Act
        var result = processor.Apply(model, posts);

        // Assert
        result.Should().OnlyContain(p => p.Title.Contains("(") || p.Title.Contains(")"));
    }

    [Fact]
    public void OrName_FilteringWorks()
    {
        // Arrange
        var posts = new Post[]
        {
            new()
            {
                Id = 20,
            },
            new()
            {
                CommentCount = 20,
            },
            new()
            {
                LikeCount = 20,
            },
        }.AsQueryable();
        var model = new StrainerModel()
        {
            Filters = "(CommentCount|LikeCount)==20",
        };
        var processor = Factory.CreateDefaultProcessor();

        // Act
        var result = processor.Apply(model, posts);

        // Assert
        result.Should().OnlyContain(p => p.CommentCount == 20 || p.LikeCount == 20);
    }

    [Fact]
    public void OrName_Works_With_EmptyNameBeingIgnored()
    {
        // Arrange
        var posts = new Post[]
        {
            new()
            {
                CommentCount = 20,
            },
            new()
            {
                LikeCount = 20,
            },
        }.AsQueryable();
        var model = new StrainerModel()
        {
            Filters = "(CommentCount|)==20",
        };
        var processor = Factory.CreateDefaultProcessor();

        // Act
        var result = processor.Apply(model, posts);

        // Assert
        result.Should().OnlyContain(p => p.CommentCount == 20);
    }

    [StrainerObject(nameof(Id))]
    private class Post
    {
        public int CommentCount { get; set; }

        public int Id { get; set; }

        public int LikeCount { get; set; }

        public string Title { get; set; }
    }
}
