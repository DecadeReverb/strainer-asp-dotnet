using FluentAssertions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;
using System;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Filtering.Operators
{
    public class LessThanOperatorTests : StrainerFixtureBase
    {
        public LessThanOperatorTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void LessThan_Works_For_Numbers()
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
                Filters = "LikeCount<3",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(c => c.LikeCount < 3);
        }

        [Fact]
        public void LessThan_Works_For_ComplexTypes()
        {
            // Arrange
            var source = new[]
            {
                new Comment
                {
                    DateTime = DateTime.UtcNow.AddDays(-3),
                },
                new Comment
                {
                    DateTime = DateTime.UtcNow.AddDays(2),
                },
            }.AsQueryable();
            var processor = Factory.CreateDefaultProcessor(options => options.ThrowExceptions = true);
            var model = new StrainerModel
            {
                Filters = $"DateTime<{DateTime.UtcNow}",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(c => c.DateTime < DateTime.UtcNow);
        }

        [Fact]
        public void LessThan_DoesNot_Work_For_StringValues()
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
                Filters = "Text<3",
            };

            // Act & Assert
            Assert.Throws<StrainerUnsupportedOperatorException>(() => processor.ApplyFiltering(model, source));
        }

        [Fact]
        public void LessThan_DoesNot_Work_For_ComplexValues_Without_DedicatedTypeConverter()
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
                Filters = "Point<3",
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
}
