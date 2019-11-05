using FluentAssertions;
using Fluorite.Strainer.Models;
using Fluorite.Strainer.Models.Sorting;
using Fluorite.Strainer.Services;
using Fluorite.Strainer.Services.Sorting;
using Moq;
using System;
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
            var customSortMethod = new CustomSortMethod<Uri>
            {
                Function = context => context
                    .Source
                    .OrderBy(uri => uri.Port)
                    .ThenBy(uri => uri.Host),
                Name = "IntThenText",
            };
            var mapper = new CustomSortMethodMapper(optionsProvider);

            // Act
            mapper.AddMap(customSortMethod);
            var result = mapper.GetMethod<Uri>(customSortMethod.Name);

            // Assert
            result.Should().NotBeNull();
            result.Should().Be(customSortMethod);
        }
    }
}
