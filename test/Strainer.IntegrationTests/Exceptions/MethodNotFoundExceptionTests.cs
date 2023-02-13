﻿using Fluorite.Strainer.Exceptions;
using Fluorite.Strainer.IntegrationTests.Fixtures;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Services;

namespace Fluorite.Strainer.IntegrationTests.Exceptions
{
    public class MethodNotFoundExceptionTests : StrainerFixtureBase
    {
        public MethodNotFoundExceptionTests(StrainerFactory factory) : base(factory)
        {

        }

        [Fact]
        public void MethodNotFoundExceptionWork()
        {
            // Arrange
            var queryable = Enumerable.Empty<int>().AsQueryable();
            var model = new StrainerModel()
            {
                Filters = "does not exist",
            };
            var processor = Factory.CreateProcessor((context) =>
            {
                context.Options.ThrowExceptions = true;

                return new StrainerProcessor(context);
            });

            // Assert
            Assert.Throws<StrainerMethodNotFoundException>(() => processor.Apply(model, queryable));
        }
    }
}
