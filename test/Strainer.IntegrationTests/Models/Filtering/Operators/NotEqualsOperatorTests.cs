﻿using FluentAssertions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.Models;
using System;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Models.Filtering.Operators
{
    public class NotEqualsOperatorTests : StrainerFixtureBase
    {
        public NotEqualsOperatorTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void NotEquals_Works_When_CaseSensivity_IsDisabled()
        {
            // Arrange
            var source = new[]
            {
                new Comment
                {
                    Text = "foo",
                },
                new Comment
                {
                    Text = "bar",
                },
                new Comment
                {
                    Text = "FOO",
                },
            }.AsQueryable();
            var processor = Factory.CreateDefaultProcessor(options => options.IsCaseInsensitiveForValues = true);
            var model = new StrainerModel
            {
                Filters = "Text!=foo",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(b => !b.Text.Equals("foo", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void NotEquals_Works_When_CaseSensivity_IsEnabled()
        {
            // Arrange
            var source = new[]
            {
                new Comment
                {
                    Text = "foo",
                },
                new Comment
                {
                    Text = "bar",
                },
                new Comment
                {
                    Text = "FOO",
                },
            }.AsQueryable();
            var processor = Factory.CreateDefaultProcessor();
            var model = new StrainerModel
            {
                Filters = "Text!=foo",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(b => !b.Text.Equals("foo", StringComparison.Ordinal));
        }

        [Fact]
        public void NotEquals_Works_For_NonStringValues()
        {
            // Arrange
            var source = new[]
            {
                new Comment
                {
                    LikeCount = 20,
                },
                new Comment
                {
                    LikeCount = 10,
                },
                new Comment
                {
                    LikeCount = 50,
                },
            }.AsQueryable();
            var processor = Factory.CreateDefaultProcessor();
            var model = new StrainerModel
            {
                Filters = "LikeCount!=20",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(c => !c.LikeCount.Equals(20));
        }
    }

    class Comment
    {
        [StrainerProperty(IsFilterable = true)]
        public int LikeCount { get; set; }

        [StrainerProperty(IsFilterable = true)]
        public string Text { get; set; }
    }
}