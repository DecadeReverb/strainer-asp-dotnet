﻿using FluentAssertions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Sorting;
using Fluorite.Strainer.TestModels;
using Moq;
using System.Linq;
using Xunit;

namespace Fluorite.Strainer.UnitTests.Services.Sorting
{
    public class CustomSortMethodMapperTests
    {
        [Fact]
        public void Mapper_Adds_NewCustomMethod()
        {
            // Arrange
            var optionsMock = new Mock<IStrainerOptionsProvider>();
            optionsMock.Setup(provider => provider.GetStrainerOptions())
                .Returns(new StrainerOptions());
            var optionsProvider = optionsMock.Object;
            var customSortMethod = new CustomSortMethod<Comment>
            {
                Function = context => context.Source
                    .OrderBy(c => c.DateCreated)
                    .ThenBy(c => c.Text),
                Name = "DateCreatedThenText",
            };
            var mapper = new CustomSortMethodMapper(optionsProvider);

            // Act
            mapper.AddMap(customSortMethod);
            var result = mapper.GetMethod<Comment>(customSortMethod.Name);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(customSortMethod);
        }
    }
}
