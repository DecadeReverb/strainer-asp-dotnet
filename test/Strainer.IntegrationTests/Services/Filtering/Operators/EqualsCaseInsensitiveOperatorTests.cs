﻿using FluentAssertions;
using Fluorite.Strainer.Attributes;
using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;
using System;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Services.Filtering.Operators
{
    public class EqualsCaseInsensitiveTests : StrainerFixtureBase
    {
        public EqualsCaseInsensitiveTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void EqualsCaseInsensitive_Works_When_CaseSensivity_IsDisabled()
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
                Filters = "Text==*foo",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(b => b.Text.Equals("foo", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void EqualsCaseInsensitive_Works_Even_When_CaseSensivity_IsEnabled()
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
                Filters = "Text==*foo",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(b => b.Text.Equals("foo", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void EqualsCaseInsensitive_Works_For_NonStringValues()
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
                Filters = "LikeCount==*20",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(c => c.LikeCount.Equals(20));
        }

        private class Comment
        {
            [StrainerProperty(IsFilterable = true)]
            public int LikeCount { get; set; }

            [StrainerProperty(IsFilterable = true)]
            public string Text { get; set; }
        }
    }
}
