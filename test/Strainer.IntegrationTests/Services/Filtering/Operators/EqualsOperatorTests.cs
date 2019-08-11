﻿using FluentAssertions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.TestModels;
using System;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.IntegrationTests.Services.Filtering.Operators
{
    public class EqualsOperatorTests : StrainerFixtureBase
    {
        public EqualsOperatorTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void Equals_Works_When_CaseSensivity_IsDisabled()
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
            var processor = Factory.CreateDefaultProcessor(options => options.IsCaseSensitiveForValues = false);
            var model = new StrainerModel
            {
                Filters = "Text==foo",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(b => b.Text.Equals("foo", StringComparison.OrdinalIgnoreCase));
        }

        [Fact]
        public void Equals_Works_When_CaseSensivity_IsEnabled()
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
            var processor = Factory.CreateDefaultProcessor(options => options.IsCaseSensitiveForValues = true);
            var model = new StrainerModel
            {
                Filters = "Text==foo",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(b => b.Text.Equals("foo", StringComparison.Ordinal));
        }

        [Fact]
        public void Equals_Works_For_NonStringValues()
        {
            // Arrange
            var source = new[]
            {
                new Post
                {
                    LikeCount = 20,
                },
                new Post
                {
                    LikeCount = 10,
                },
                new Post
                {
                    LikeCount = 50,
                },
            }.AsQueryable();
            var processor = Factory.CreateDefaultProcessor(options => options.IsCaseSensitiveForValues = true);
            var model = new StrainerModel
            {
                Filters = "LikeCount==20",
            };

            // Act
            var result = processor.ApplyFiltering(model, source);

            // Assert
            result.Should().OnlyContain(p => p.LikeCount.Equals(20));
        }
    }
}
