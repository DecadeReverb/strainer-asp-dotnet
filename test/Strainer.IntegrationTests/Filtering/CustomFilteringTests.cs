using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services.Modules;

namespace Fluorite.Strainer.IntegrationTests.Filtering;

public class CustomFilteringTests : StrainerFixtureBase
{
    public CustomFilteringTests(StrainerFactory factory) : base(factory)
    {

    }

    [Fact]
    public void CustomFiltersWork()
    {
        // Arrange
        var queryable = new List<Post>
        {
            new()
            {
                LikeCount = 100,
            },
            new()
            {
                LikeCount = 101,
            },
        }.AsQueryable();
        var model = new StrainerModel()
        {
            Filters = "IsPopular",
        };
        var processor = Factory.CreateDefaultProcessor<TestStrainerModule>();

        // Act
        var result = processor.Apply(model, queryable);

        // Assert
        result.Should().OnlyContain(p => p.LikeCount > 100);
    }

    [Fact]
    public void CustomFiltersWithOperatorsWork()
    {
        // Arrange
        var queryable = new List<Post>
        {
            new()
            {
                Title = "Foo",
            },
            new()
            {
                Title = "Equals in C# is \'==\'.",
            },
        }.AsQueryable();
        var model = new StrainerModel()
        {
            Filters = "HasInTitleFilterOperator==",
        };
        var processor = Factory.CreateDefaultProcessor<TestStrainerModule>();

        // Act
        var result = processor.Apply(model, queryable);

        // Assert
        result.Should().OnlyContain(p => p.Title.Contains("=="));
    }

    [Fact]
    public void CustomFiltersMixedWithRegularFilters()
    {
        // Arrange
        var queryable = new List<Post>
        {
            new()
            {
                CommentCount = 10,
                LikeCount = 200,
            },
            new()
            {
                CommentCount = 0,
                LikeCount = 200,
            },
        }.AsQueryable();
        var model = new StrainerModel()
        {
            Filters = "IsPopular,CommentCount>0",
        };
        var processor = Factory.CreateDefaultProcessor<TestStrainerModule>();

        // Act
        var result = processor.Apply(model, queryable);

        // Assert
        result.Should().OnlyContain(p => p.LikeCount > 100);
        result.Should().OnlyContain(p => p.CommentCount > 0);
    }

    [Fact]
    public void CustomFiltersMixedRegularFiltersOtherWayAround()
    {
        // Arrange
        var queryable = new List<Post>
        {
            new()
            {
                CommentCount = 0,
                LikeCount = 200,
            },
            new()
            {
                CommentCount = 2,
                LikeCount = 200,
            },
        }.AsQueryable();
        var model = new StrainerModel()
        {
            Filters = "CommentCount==2,IsPopular",
        };
        var processor = Factory.CreateDefaultProcessor<TestStrainerModule>();

        // Act
        var result = processor.Apply(model, queryable);

        // Assert
        result.Should().OnlyContain(p => p.CommentCount == 2 && p.LikeCount > 100);
    }

    [Fact]
    public void CustomFiltersOnDifferentSourcesCanShareName()
    {
        // Arrange
        var queryable = new List<Post>
        {
            new()
            {
                LikeCount = 150,
                DateCreated = DateTime.UtcNow.AddDays(-7),
            },
            new()
            {
                LikeCount = 200,
                DateCreated = DateTime.UtcNow,
            },
        }.AsQueryable();
        var postModel = new StrainerModel()
        {
            Filters = "LikeCount==200,IsNew",
        };
        var processor = Factory.CreateDefaultProcessor<TestStrainerModule>();

        // Act
        var postResult = processor.Apply(postModel, queryable);

        // Assert
        postResult.Should().OnlyContain(p => p.LikeCount == 200);
        postResult.Should().OnlyContain(p => p.DateCreated > DateTime.UtcNow.AddDays(-2));

        // ###

        // Arrange
        var secondModel = new StrainerModel()
        {
            Filters = "IsNew",
        };

        // Act
        var secondResult = processor.Apply(secondModel, queryable);

        // Assert
        secondResult.Should().OnlyContain(c => c.DateCreated > DateTimeOffset.UtcNow.AddDays(-2));
    }

    private class Post
    {
        public int CommentCount { get; set; }

        public DateTime DateCreated { get; set; }

        public int LikeCount { get; set; }

        public string Title { get; set; }
    }

    private class TestStrainerModule : StrainerModule
    {
        public override void Load(IStrainerModuleBuilder builder)
        {
            builder
                .AddProperty<Post>(p => p.CommentCount)
                .IsFilterable();

            builder
                .AddProperty<Post>(p => p.LikeCount)
                .IsFilterable()
                .IsSortable()
                .IsDefaultSort();

            builder
                .AddCustomFilterMethod<Post>(b => b
                    .HasName("HasInTitleFilterOperator")
                    .HasFunction(term => p => p.Title.Contains(term.Operator.Symbol))
                    .Build());
            builder
                .AddCustomFilterMethod<Post>(b => b
                    .HasName("IsNew")
                    .HasFunction(c => c.DateCreated > DateTimeOffset.UtcNow.AddDays(-2))
                    .Build());
            builder
                .AddCustomFilterMethod<Post>(b => b
                    .HasName("IsPopular")
                    .HasFunction(p => p.LikeCount > 100)
                    .Build());
        }
    }
}
