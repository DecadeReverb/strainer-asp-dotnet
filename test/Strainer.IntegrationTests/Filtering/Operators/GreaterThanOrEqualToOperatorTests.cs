using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;

namespace Fluorite.Strainer.IntegrationTests.Filtering.Operators;

public class GreaterThanOrEqualToOperatorTests : StrainerFixtureBase
{
    public GreaterThanOrEqualToOperatorTests(StrainerFactory factory) : base(factory)
    {

    }

    [Fact]
    public void GreaterThanOrEqualTo_Works_For_Numbers()
    {
        // Arrange
        var source = new[]
        {
            new Comment
            {
                LikeCount = 2,
            },
            new Comment
            {
                LikeCount = 3,
            },
        }.AsQueryable();
        var processor = Factory.CreateDefaultProcessor();
        var model = new StrainerModel
        {
            Filters = "LikeCount>=2",
        };

        // Act
        var result = processor.ApplyFiltering(model, source);

        // Assert
        result.Should().OnlyContain(c => c.LikeCount >= 2);
    }

    [Fact]
    public void GreaterThanOrEqualTo_Works_For_ComplexTypes()
    {
        // Arrange
        var dateTimeNow = DateTime.UtcNow;
        var source = new[]
        {
            new Comment
            {
                DateTime = DateTime.UtcNow.AddDays(-3),
            },
            new Comment
            {
                DateTime = dateTimeNow,
            },
        }.AsQueryable();
        var processor = Factory.CreateDefaultProcessor(options => options.ThrowExceptions = true);
        var model = new StrainerModel
        {
            Filters = $"DateTime>={dateTimeNow}",
        };

        // Act
        var result = processor.ApplyFiltering(model, source);

        // Assert
        result.Should().OnlyContain(c => c.DateTime >= dateTimeNow);
    }

    [Fact]
    public void GreaterThanOrEqualTo_DoesNot_Work_For_StringValues()
    {
        // Arrange
        var source = new[]
        {
            new Comment
            {
                Text = "2",
            },
            new Comment
            {
                Text = "3",
            },
        }.AsQueryable();
        var processor = Factory.CreateDefaultProcessor(options => options.ThrowExceptions = true);
        var model = new StrainerModel
        {
            Filters = "Text>=2",
        };

        // Act & Assert
        Assert.Throws<StrainerUnsupportedOperatorException>(() => processor.ApplyFiltering(model, source));
    }

    [Fact]
    public void GreaterThanOrEqualTo_DoesNot_Work_For_ComplexValues_Without_DedicatedTypeConverter()
    {
        // Arrange
        var source = new[]
        {
            new Comment
            {
                Point = new Point(2),
            },
            new Comment
            {
                Point = new Point(3),
            },
        }.AsQueryable();
        var processor = Factory.CreateDefaultProcessor(options => options.ThrowExceptions = true);
        var model = new StrainerModel
        {
            Filters = "Point>=2",
        };

        // Act & Assert
        Assert.Throws<StrainerConversionException>(() => processor.ApplyFiltering(model, source));
    }

    private class Comment
    {
        [StrainerProperty]
        public int LikeCount { get; set; }

        [StrainerProperty]
        public Point Point { get; set; }

        [StrainerProperty]
        public string Text { get; set; }

        [StrainerProperty]
        public DateTime DateTime { get; set; }
    }

    private readonly struct Point
    {
        public Point(int value) => Value = value;

        public int Value { get; }

        public static bool operator <(Point point1, Point point2)
        {
            return point1.Value < point2.Value;
        }

        public static bool operator >(Point point1, Point point2)
        {
            return point1.Value > point2.Value;
        }
    }
}
